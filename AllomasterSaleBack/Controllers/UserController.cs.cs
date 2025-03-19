using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlloMasterSale.Data;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Company)
            .Select(u => new
            {
                u.Id,
                u.Login,
                u.Role,
                Company = new { u.Company.Id, u.Company.Name }
            })
            .ToListAsync();

        return Ok(users);
    }
}