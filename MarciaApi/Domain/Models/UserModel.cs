using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class UserModel
{
    [Key]
    public string? Id { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; }
    public List<Order>? Orders { get; set; }
}