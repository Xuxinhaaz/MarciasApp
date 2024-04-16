using MarciaApi.Domain.Models;

namespace MarciaApi.Presentation.ViewModel.Orders;

public class OrdersViewModel
{
    public string? UserName { get; set; }   
    public string? UserPhone { get; set; }
    public List<string>? ProductsNames { get; set; }
    public Location? Location { get; set; }
    public string? PaymentType { get; set; }
}