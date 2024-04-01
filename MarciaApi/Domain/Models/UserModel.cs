using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class UserModel
{
    [Key]
    public string? Id { get; set; }
    public string? Email { get; set; }
    public DateTime WhenWasCreated { get; set; } = DateTime.UtcNow;
    public List<Roles> Roles { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}