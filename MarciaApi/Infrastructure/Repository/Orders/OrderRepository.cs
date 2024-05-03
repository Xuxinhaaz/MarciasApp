using System.Linq.Expressions;
using System.Net;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.ViewModel.Orders;

namespace MarciaApi.Infrastructure.Repository.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IGenericRepository<Order, OrderDto> _genericRepository;

    public OrderRepository(IGenericRepository<Order, OrderDto> genericRepository, IUserRepository userRepository, AppDbContext context, IProductsRepository productsRepository)
    {
        _genericRepository = genericRepository;
        _userRepository = userRepository;
        _context = context;
        _productsRepository = productsRepository;
    } 
    
    public async Task<OrderDto> Get(Expression<Func<Order, bool>> filter)
    {
        Order orders = await _genericRepository.Get(filter);
        OrderDto dto = await _genericRepository.Map(orders);

        return dto;
    }

    public async Task<List<OrderDto>> Get(
        int pageNumber, 
        Expression<Func<Order, bool>> filter = null, 
        params Expression<Func<Order, object>>[] includes)
    {
        var orders = await _genericRepository.Get(pageNumber, filter, includes);
        var dtos = await _genericRepository.Map(orders);

        return dtos;
    }

    public async Task<Order?> Generate(string id, OrdersViewModel model)
    {
        var userFound = await _userRepository.Get(id);

        foreach (var item in model.ProductsNames)
        {
            Console.WriteLine(item);
            if(!await _productsRepository.Any(x => x.ProductName == item)) return null; 
        }
        var productsFound = await _productsRepository.GetByName(model.ProductsNames);
        
        var newOrder = new Order
        {
            OrderId = Guid.NewGuid().ToString(),
            Location = model.Location,
            IsPaid = false,
            Products = productsFound,
            UserName = model.UserName,
            UserPhone = model.UserPhone,
            PaymentType = model.PaymentType,
            UsersId = userFound.Id
        };

        model.Location.LocationId = Guid.NewGuid().ToString();
        model.Location.OrderId = newOrder.OrderId;
        
        foreach (var product in productsFound)
        {
            product.Orders.Add(newOrder);
        }
        
        userFound.Orders.Add(newOrder);
        _genericRepository.Add(newOrder);
        await _context.SaveChangesAsync();
        
        return newOrder;
    }   

    public async Task Delete(Expression<Func<Order, bool>> filter)
    {
        var orderFound = await _genericRepository.Get(filter);

        await _genericRepository.Delete(orderFound);
    }

    public async Task<bool> DeleteAnOrderByUserId(string userId, string orderId)
    {
        var userFound = await _userRepository.Get(userId);
        var orderFound = await _genericRepository.Get(x => x.OrderId == orderId);
        
        if(userFound.Id != orderFound.UsersId) return false; 

        await _genericRepository.Delete(orderFound);

        return true;
    }

    public async Task<bool> Any(Expression<Func<Order, bool>> filter)
    {
        return await _genericRepository.Any(filter);
    }

}