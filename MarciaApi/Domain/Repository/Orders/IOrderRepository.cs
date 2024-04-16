using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.EntityFrameworkCore.Storage;

namespace MarciaApi.Domain.Repository.Orders;

public interface IOrderRepository
{
    Task<List<OrderDto>> Get(int pageNumber);
    Task<List<OrderDto>> GetByUserId(string id, int pageNumber);
    Task<Order> Generate(string id, OrdersViewModel model);
    Task Delete(string UserId, Expression<Func<Order, bool>> filter);
    Task<bool> DeleteAnOrderByUserId(string id, string orderId);
    Task<bool> Any(string id, Expression<Func<Order, bool>> filter);
}