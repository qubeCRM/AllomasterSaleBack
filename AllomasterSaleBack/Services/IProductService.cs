using AlloMasterSale.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
    }
}