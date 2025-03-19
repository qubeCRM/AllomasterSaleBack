using Microsoft.AspNetCore.Mvc;
using AlloMasterSale.Models;
using AlloMasterSale.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AlloMasterSale.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result.Contains("существует"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == "Неверный логин или пароль")
            {
                return Unauthorized(new { message = token });
            }

            return Ok(new { token });
        }

        [HttpPost("register-manager")]
        [Authorize(Roles = "Admin")] // Доступ только для администратора
        public async Task<IActionResult> RegisterManager([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterManagerAsync(dto);
            if (result.Contains("существует"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

    }
}