using MarciaApi.Domain.Models;

namespace MarciaApi.Domain.Repository.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly IGenericRepository<Order> _genericRepository;

    public OrderRepository(IGenericRepository<Order> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<Order> Generate()
    {
        return new Order();
    }
}