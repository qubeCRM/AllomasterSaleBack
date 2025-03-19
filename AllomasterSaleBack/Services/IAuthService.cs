using System.Threading.Tasks;
using AlloMasterSale.Models;

namespace AlloMasterSale.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto); // ✅ Должен возвращать Task<string>
    }
}