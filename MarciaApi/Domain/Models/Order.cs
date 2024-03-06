using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarciaApi.Domain.Models;

public class Order
{
    [Key]
    public string? OrderId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string? UserName { get; set; }   
    public string? UserPhone { get; set; }
    public double? TotalPrice { get; set; }
    public bool? IsPaid { get; set; }
    [ForeignKey("Id")]
    public string? UsersId { get; set; }
  
}