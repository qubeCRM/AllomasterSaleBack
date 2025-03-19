using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AlloMasterSale.Data;
using AlloMasterSale.Services;
using AlloMasterSale.Models;
using Microsoft.Extensions.Logging;

namespace AlloMasterSale.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            AppDbContext context,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPayments(
            [FromQuery] string status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _logger.LogInformation("Получен запрос на получение платежей: UserId={UserId}, Status={Status}, Page={Page}, PageSize={PageSize}", userId, status, page, pageSize);

                var user = await _context.Users.Include(u => u.Company).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("Пользователь не найден");
                    return Unauthorized();
                }

                var query = _context.Payments
                    .Where(p => p.Subscription.CompanyId == user.CompanyId);

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                var totalCount = await query.CountAsync();
                var payments = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new PaymentDto
                    {
                        Id = p.Id,
                        SubscriptionId = p.SubscriptionId,
                        Amount = p.Amount,
                        Currency = p.Currency,
                        Status = p.Status,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                _logger.LogInformation("Успешно получено {Count} платежей для пользователя {UserId}", payments.Count, userId);
                return Ok(new
                {
                    totalCount,
                    page,
                    pageSize,
                    payments
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении платежей: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}