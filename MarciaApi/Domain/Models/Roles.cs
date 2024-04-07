using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class Roles
{
    [Key] 
    public string RoleId { get; set; }
    public string Role { get; set; }
    public List<UserModel> UserModels { get; set; } = new();
}