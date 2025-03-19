using AlloMasterSale.Data;
using AlloMasterSale.Models;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(int subscriptionId, decimal amount);
        Task<List<PaymentDto>> GetPaymentsByCompanyIdAsync(int companyId);
    }
}