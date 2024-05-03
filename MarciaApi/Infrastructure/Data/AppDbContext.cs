using MarciaApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<UserModel> Users { get; set; }
}