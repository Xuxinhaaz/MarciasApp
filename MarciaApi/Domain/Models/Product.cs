using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarciaApi.Domain.Models;

public class Product
{
    [Key]
    public string? ProdutId { get; set; }
    public string? ProductName { get; set; }
    [JsonIgnore]
    public List<Order>? Orders { get; set; } = new();
    [JsonIgnore]
    public List<Size>? Sizes { get; set; } = new();
    [JsonIgnore]
    public List<Item>? Items { get; set; } = new();
    public string? ProductDescription { get; set; }
    public double? TotalProductPrice { get; set; }
}