using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.Orders;

namespace MarciaApi.Domain.Repository.Orders;

public interface IOrderRepository
{
    Task<List<Order>> Get(int pageNumber);
    Task<Order> Generate(string id, OrdersViewModel model);
}