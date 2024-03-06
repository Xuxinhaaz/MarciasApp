using System.ComponentModel.DataAnnotations;
using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.DTOs.User;

public class UserModelDto
{
    [Key]
    public string? Id { get; set; }
    public string? Email { get; set; }
    public List<Order>? Orders { get; set; }
    public string? JwtToken { get; set; }
}