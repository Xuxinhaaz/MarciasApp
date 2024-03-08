using System.ComponentModel.DataAnnotations.Schema;
using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.DTOs.Orders;

public class OrderDto
{
    public string? OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string? UserName { get; set; }   
    public string? UserPhone { get; set; }
    public double? TotalPrice { get; set; }
    public bool? IsPaid { get; set; }
    public string? PaymentType { get; set; }
    public List<Product>? Products { get; set; }
    public Location? Location { get; set; }
    public string? UsersId { get; set; }
}