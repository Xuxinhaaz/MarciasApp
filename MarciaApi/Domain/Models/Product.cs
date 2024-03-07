using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class Product
{
    [Key]
    public string? ProdutId { get; set; }
    public string? ProductName { get; set; }
    public List<Order>? Orders { get; set; }
    public List<Size>? Sizes { get; set; }
    public List<Item>? AdditionalItems { get; set; }
    public string? ProductDescription { get; set; }
    public double? TotalProductPrice { get; set; }
}