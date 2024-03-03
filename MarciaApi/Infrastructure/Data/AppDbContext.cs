using MarciaApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> Users { get; set; }
}