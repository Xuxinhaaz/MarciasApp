using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarciaApi.Domain.Repository.Orders;

public interface IOrderRepository
{
    Task<OrderDto> Get(Expression<Func<Order, bool>> filter);
    Task<List<OrderDto>> Get(int pageNumber, Expression<Func<Order, bool>> filter, params Expression<Func<Order, object>>[] includes);
    Task<Order> Generate(string id, OrdersViewModel model);
    Task Delete(Expression<Func<Order, bool>> filter);
    Task<bool> DeleteAnOrderByUserId(string id, string orderId);
    Task<bool> Any(Expression<Func<Order, bool>> filter);
}