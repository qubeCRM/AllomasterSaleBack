using AlloMasterSale.Models;
using System.Threading.Tasks;

namespace AlloMasterSale.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> RegisterManagerAsync(RegisterDto dto); // Новый метод
        Task<string> LoginAsync(LoginDto dto);
    }
}