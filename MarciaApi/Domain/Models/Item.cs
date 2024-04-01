using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class Item
{
    [Key]
    public string? ItemId { get; set; }
    public string? ItemName { get; set; }
    public double? ItemPrice { get; set; }
    public List<Product>? Products { get; set; } = new();
}