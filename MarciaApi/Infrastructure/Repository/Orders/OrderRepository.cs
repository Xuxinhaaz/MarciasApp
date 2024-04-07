using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.ViewModel.Orders;

namespace MarciaApi.Infrastructure.Repository.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<Order, OrderDto> _genericRepository;

    public OrderRepository(IGenericRepository<Order, OrderDto> genericRepository, IUserRepository userRepository, AppDbContext context)
    {
        _genericRepository = genericRepository;
        _userRepository = userRepository;
        _context = context;
    } 
    
    public async Task<List<OrderDto>> Get(int pageNumber)
    {
        List<Order> orders = await _genericRepository.Get(pageNumber);
        List<OrderDto> dtos = await _genericRepository.Map(orders);

        return dtos;
    }

    public async Task<Order> Generate(string id, OrdersViewModel model)
    {
        var userFound = await _userRepository.Get(id);

        var newOrder = new Order
        {
            OrderId = Guid.NewGuid().ToString(),
            Location = model.Location,
            Products = model.Products,
            IsPaid = false,
            UserName = model.UserName,
            UserPhone = model.UserPhone,
            UsersId = userFound.Id
        };
        
        model.Location.LocationId = Guid.NewGuid().ToString();
        model.Location.OrderId = newOrder.OrderId;
        
        userFound.Orders.Add(newOrder);
        _genericRepository.Add(newOrder);
        await _context.SaveChangesAsync();
        
        return newOrder;
    }
}