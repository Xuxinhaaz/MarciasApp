using MarciaApi.Presentation.ViewModel.Items;

namespace MarciaApi.Presentation.ViewModel.Products;

public class ProductsOrderViewModel
{
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public List<ItemsProductsOrdersViewModel>? Items { get; set; }
}