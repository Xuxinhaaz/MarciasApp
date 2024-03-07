using System.ComponentModel.DataAnnotations;

namespace MarciaApi.Domain.Models;

public class Size
{
    [Key]
    public string? SizeId { get; set; }
    public string? SizeName { get; set; }
    public double? SizePrice { get; set; }
    public List<Product>? Products { get; set; } = new();
}