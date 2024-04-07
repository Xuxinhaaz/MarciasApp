using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarciaApi.Domain.Models;

public class Order
{
    [Key]
    public string? OrderId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? UserName { get; set; }   
    public string? UserPhone { get; set; }
    public double? TotalPrice { get; set; }
    public bool? IsPaid { get; set; }
    public string? PaymentType { get; set; }
    public List<Product>? Products { get; set; } = new();
    public Location? Location { get; set; }
    [ForeignKey("Id")]
    public string? UsersId { get; set; }
  
}