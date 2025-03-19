using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlloMasterSale.Data;
using AlloMasterSale.Models;

namespace AlloMasterSale.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(int subscriptionId, decimal amount)
        {
            var payment = new Payment
            {
                SubscriptionId = subscriptionId,
                Amount = amount,
                PaymentGatewayId = "gateway_id_placeholder",
                Currency = "KZT",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            payment.Status = "Completed";
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<List<PaymentDto>> GetPaymentsByCompanyIdAsync(int companyId)
        {
            return await _context.Payments
                .Where(p => p.Subscription.CompanyId == companyId)
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
        }
    }
}