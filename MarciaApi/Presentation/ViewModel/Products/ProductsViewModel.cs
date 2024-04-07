using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.ViewModel.Products;

public class ProductsViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? ItemsNames { get; set; }
    public double? Price { get; set; }
}