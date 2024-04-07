using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.DTOs.Items;

public class ItemDto
{
    public string? ItemId { get; set; }
    public string? ItemName { get; set; }
    public double? ItemPrice { get; set; }
    public List<Product>? Products { get; set; } = new();
}