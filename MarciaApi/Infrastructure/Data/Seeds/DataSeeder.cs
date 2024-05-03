using MarciaApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Data.Seeds;

public class DataSeeder
{
    private readonly AppDbContext _context;
    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Roles.Any())
        {
            var roles = new List<Roles>()
            {
                new Roles { UserModels = new List<UserModel>(), RoleId = Guid.NewGuid().ToString(), Role = "Client" },
                new Roles { UserModels = new List<UserModel>(), RoleId = Guid.NewGuid().ToString(), Role = "Manager" }
            };

            _context.AddRange(roles);
            _context.SaveChanges();
        }
    }
}