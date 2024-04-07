using System.Text.Json.Serialization;
using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.DTOs.Products;

public class ProductDto
{
    public string? ProdutId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public double? TotalProductPrice { get; set; }
    public List<Item>? Items { get; set; } = new();
}