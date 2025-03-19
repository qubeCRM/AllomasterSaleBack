using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlloMasterSale.Data;
using AlloMasterSale.Models;
using AlloMasterSale.Helpers;

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

            if (dto.CompanyId.HasValue && dto.CompanyId.Value > 0) // Проверяем, что CompanyId больше 0
            {
                company = await _context.Companies.FindAsync(dto.CompanyId.Value);
                if (company == null)
                {
                    return "Компания с указанным ID не найдена";
                }
                companyId = company.Id;
            }
            else if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                // Проверяем, существует ли компания с таким названием
                company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == dto.CompanyName);
                if (company != null)
                {
                    companyId = company.Id; // Используем существующую компанию
                }
                else
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
            }
            else
            {
                return "Необходимо указать ID компании или создать новую";
            }

            var user = new User
            {
                Login = dto.Login,
                Password = PasswordHelper.HashPassword(dto.Password),
                Role = "User",
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

            var token = _jwtHelper.GenerateToken(user.Id.ToString(), user.Login, user.Role);
            return token;
        }

        public async Task<string> RegisterManagerAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Login == dto.Login))
            {
                return "Пользователь с таким логином уже существует";
            }

            int companyId;
            Company company;

            if (dto.CompanyId.HasValue && dto.CompanyId.Value > 0)
            {
                company = await _context.Companies.FindAsync(dto.CompanyId.Value);
                if (company == null)
                {
                    return "Компания с указанным ID не найдена";
                }
                companyId = company.Id;
            }
            else if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == dto.CompanyName);
                if (company != null)
                {
                    companyId = company.Id;
                }
                else
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
            }
            else
            {
                return "Необходимо указать ID компании или создать новую";
            }

            var user = new User
            {
                Login = dto.Login,
                Password = PasswordHelper.HashPassword(dto.Password),
                Role = "Manager",
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return $"Регистрация менеджера успешна, компания ID: {companyId}";
        }
    }
}