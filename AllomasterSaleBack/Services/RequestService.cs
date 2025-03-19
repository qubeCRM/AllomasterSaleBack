using AlloMasterSale.Data;
using AlloMasterSale.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _context;

        public RequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Request> CreateRequestAsync(int userId, int companyId, int subscriptionId, int paymentId)
        {
            var request = new Request
            {
                UserId = userId,
                CompanyId = companyId,
                SubscriptionId = subscriptionId,
                PaymentId = paymentId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<List<RequestDto>> GetAllRequestsAsync()
        {
            try
            {
                Console.WriteLine("Начало загрузки заявок...");
                var requests = await _context.Requests
                    .Include(r => r.User)
                        .ThenInclude(u => u.Company)
                    .Include(r => r.Company)
                    .Include(r => r.Subscription)
                    .Include(r => r.Payment)
                    .Select(r => new RequestDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        UserLogin = r.User.Login,
                        CompanyId = r.CompanyId,
                        CompanyName = r.Company.Name,
                        SubscriptionId = r.SubscriptionId,
                        SubscriptionMonths = r.Subscription.Months,
                        PaymentId = r.PaymentId,
                        PaymentAmount = r.Payment.Amount,
                        Status = r.Status,
                        CreatedAt = r.CreatedAt
                    })
                    .ToListAsync();

                Console.WriteLine($"Загружено {requests.Count} заявок");
                return requests;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заявок: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            var request = await _context.Requests
                .Include(r => r.Subscription)
                .FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null)
            {
                throw new Exception("Заявка не найдена");
            }

            request.Status = "Approved";
            request.Subscription.IsActive = true;
            await _context.SaveChangesAsync();
        }

        public async Task RejectRequestAsync(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if (request == null)
            {
                throw new Exception("Заявка не найдена");
            }

            request.Status = "Rejected";
            await _context.SaveChangesAsync();
        }
    }
}