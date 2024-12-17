using System.Linq.Expressions;
using ErrorOr;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarciaApi.Domain.Repository.Orders;

public interface IOrderRepository
{
    void ReplaceCep(OrdersViewModel viewModel);
    Task<OrderDto> Get(Expression<Func<Order, bool>> filter);
    Task<List<OrderDto>> Get(int pageNumber, Expression<Func<Order, bool>>? filter, params Expression<Func<Order, object>>[] includes);
    Task<ErrorOr<OrderDto>> Generate(string id, OrdersViewModel model);
    Task<ErrorOr<bool>> Delete(Expression<Func<Order, bool>> filter);
    Task<ErrorOr<bool>> DeleteAnOrderByUserId(string id, string orderId);
    Task<bool> Any(Expression<Func<Order, bool>> filter);
    Task<(double? finalPrice, List<Item>? RemovedItems, List<Product> products)> CalculatingPrice(OrdersViewModel model);
}