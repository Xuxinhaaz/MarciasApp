using MarciaApi.Domain.Models;
using MarciaApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class RolesManagerController
{
    private readonly AppDbContext _context;

    public RolesManagerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("/Roles")]
    public async Task<IActionResult> PostRole([FromQuery]string role)
    {
        var newRole = new Roles()
        {
            Role = role,
            RoleId = Guid.NewGuid().ToString(),
            UserModels = new List<UserModel>()
        };

        await _context.Roles.AddAsync(newRole);
        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            role = newRole
        });
    }
}