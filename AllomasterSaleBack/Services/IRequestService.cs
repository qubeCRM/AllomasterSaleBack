using System.Collections.Generic;
using System.Threading.Tasks;
using AlloMasterSale.Data;
using AlloMasterSale.Models;

namespace AlloMasterSale.Services
{
    public interface IRequestService
    {
        Task<List<RequestDto>> GetAllRequestsAsync();
        Task<Request> CreateRequestAsync(int userId, int companyId, int subscriptionId, int paymentId);
        Task ApproveRequestAsync(int requestId);
        Task RejectRequestAsync(int requestId);
    }
}