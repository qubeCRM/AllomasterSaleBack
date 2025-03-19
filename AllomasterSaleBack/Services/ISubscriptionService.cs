using AlloMasterSale.Data;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public interface ISubscriptionService
    {
        Task<Subscription> CreateSubscriptionAsync(int companyId, int productId);
    }
}