using AlloMasterSale.Data;
using System;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;

        public SubscriptionService(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<Subscription> CreateSubscriptionAsync(int companyId, int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Продукт не найден");
            }

            var subscription = new Subscription
            {
                CompanyId = companyId,
                Months = product.Months,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(product.Months),
                IsActive = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }
    }
}