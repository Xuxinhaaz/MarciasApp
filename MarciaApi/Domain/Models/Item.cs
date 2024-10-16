using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarciaApi.Domain.Models;

public class Item
{
    [Key]
    public string? ItemId { get; set; }
    public string? ItemName { get; set; }
    public double? ItemPrice { get; set; }
    [JsonIgnore]
    public List<Product>? Products { get; set; } = new();
}