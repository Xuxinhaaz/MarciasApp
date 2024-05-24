using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Presentation.ViewModel.Orders;

public class OrdersViewModel
{
    public string? UserName { get; set; }   
    public string? UserPhone { get; set; }
    public List<ProductsOrderViewModel>? Products { get; set; }
    public Location? Location { get; set; }
    public string? PaymentType { get; set; }
}