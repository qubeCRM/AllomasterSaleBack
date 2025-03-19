using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlloMasterSale.Data;
using AlloMasterSale.Models;
using AlloMasterSale.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlloMasterSale.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPaymentService _paymentService;
        private readonly IRequestService _requestService;
        private readonly IProductService _productService;
        private readonly AppDbContext _context;

        public SubscriptionController(
            ISubscriptionService subscriptionService,
            IPaymentService paymentService,
            IRequestService requestService,
            IProductService productService,
            AppDbContext context)
        {
            _subscriptionService = subscriptionService;
            _paymentService = paymentService;
            _requestService = requestService;
            _productService = productService;
            _context = context;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _context.Users.Include(u => u.Company).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return Unauthorized();

                var product = await _productService.GetProductByIdAsync(dto.ProductId);
                if (product == null) return BadRequest(new { message = "Продукт не найден" });

                var subscription = await _subscriptionService.CreateSubscriptionAsync(user.CompanyId, dto.ProductId);
                var payment = await _paymentService.CreatePaymentAsync(subscription.Id, product.Price);
                var request = await _requestService.CreateRequestAsync(user.Id, user.CompanyId, subscription.Id, payment.Id);

                return Ok(new { message = "Подписка и заявка созданы, ожидайте подтверждения менеджера" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserSubscriptions()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _context.Users.Include(u => u.Company).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null) return Unauthorized();

                var subscriptions = await _context.Subscriptions
                    .Where(s => s.CompanyId == user.CompanyId)
                    .Select(s => new SubscriptionDto
                    {
                        Id = s.Id,
                        CompanyId = s.CompanyId,
                        CompanyName = s.Company.Name,
                        Months = s.Months,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedAt
                    })
                    .ToListAsync();

                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}