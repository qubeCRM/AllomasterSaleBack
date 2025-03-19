using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlloMasterSale.Data;
using AlloMasterSale.Models;
using AlloMasterSale.Helpers;
using Microsoft.Extensions.Configuration;

namespace AlloMasterSale.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Login == dto.Login))
            {
                return "Пользователь с таким логином уже существует";
            }

            int companyId;
            Company company;

            if (dto.CompanyId.HasValue)
            {
                company = await _context.Companies.FindAsync(dto.CompanyId.Value);
                if (company == null)
                {
                    company = new Company 
                    { 
                        Name = $"Company_{dto.CompanyId}",
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                }
                companyId = company.Id;
            }
            else if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                company = new Company 
                { 
                    Name = dto.CompanyName,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                companyId = company.Id;
            }
            else
            {
                return "Необходимо указать ID компании или создать новую";
            }

            var user = new User
            {
                Login = dto.Login,
                Password = PasswordHelper.HashPassword(dto.Password),
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return $"Регистрация успешна, компания ID: {companyId}";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Password))
            {
                return "Неверный логин или пароль";
            }

            var token = _jwtHelper.GenerateToken(user.Id.ToString(), user.Login);

            return token; // ✅ Теперь метод возвращает `string`, а не `object`
        }
    }
}
