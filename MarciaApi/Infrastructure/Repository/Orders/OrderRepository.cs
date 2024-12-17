using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ErrorOr;
using MarciaApi.Domain.Data.Cloud;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Infrastructure.Repository.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ICloudflare _cloudflare;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<Product, ProductDto> _productsGenericRepository;
    private readonly IGenericRepository<Order, OrderDto> _genericRepository;
    private readonly IGenericRepository<Item, ItemDto> _genericItemRepository;

    public OrderRepository(
        IGenericRepository<Order, OrderDto> genericRepository, 
        IUserRepository userRepository,
        AppDbContext context, 
        IGenericRepository<Product, ProductDto> productsGenericRepository, IGenericRepository<Item, ItemDto> genericItemRepository, ICloudflare cloudflare)
    {
        _genericRepository = genericRepository;
        _userRepository = userRepository;
        _context = context;
        _productsGenericRepository = productsGenericRepository;
        _genericItemRepository = genericItemRepository;
        _cloudflare = cloudflare;
    }

    public void ReplaceCep(OrdersViewModel viewModel)
    {
        var formattedCep = Regex
            .Replace(viewModel.Location?.CEP ?? string.Empty, @"(\d{5})[-\s](\d{3})", "$1$2");

        viewModel.Location!.CEP = formattedCep;
    }

    public async Task<OrderDto> Get(Expression<Func<Order, bool>> filter)
    {
        Order orders = await _genericRepository.Get(filter);
        OrderDto dto = await _genericRepository.Map(orders);

        return dto;
    }

    public async Task<List<OrderDto>> Get(
        int pageNumber, 
        Expression<Func<Order, bool>>? filter = null, 
        params Expression<Func<Order, object>>[] includes)
    {
        var orders = await _genericRepository.Get(pageNumber, filter, includes);
        var dtos = await _genericRepository.Map(orders);

        return dtos;
    }

    public async Task<ErrorOr<OrderDto>> Generate(string id, OrdersViewModel model)
    {
        var userFound = await _userRepository.Get(id);
        if (userFound.IsError)
            return userFound.Errors;
        
        (double? finalPrice, List<Item>? RemovedItems, List<Product> products) valueTuple = await CalculatingPrice(model);
        
        var newOrder = new Order
        {
            OrderId = Guid.NewGuid().ToString(),
            Location = model.Location,
            IsPaid = false,
            UserName = model.UserName,
            UserPhone = model.UserPhone,
            PaymentType = model.PaymentType,
            TotalPrice = valueTuple.finalPrice,
            UsersId = userFound.Value.Id,
            RemovedItems = valueTuple.RemovedItems,
            Products = valueTuple.products,
            OrderDate = DateTime.UtcNow
        };

        if (model.Location != null)
        {
            model.Location.LocationId = Guid.NewGuid().ToString();
            model.Location.OrderId = newOrder.OrderId;
        }

        userFound.Value.Orders?.Add(newOrder);
        await _genericRepository.Add(newOrder);
        await _context.SaveChangesAsync();

        var orderDto = await _genericRepository.Map(newOrder);
        
        return orderDto;
    }   

    public async Task<ErrorOr<bool>> Delete(Expression<Func<Order, bool>> filter)
    {
        var orderFound = await _genericRepository.Get(filter);
        if (orderFound == null!)
            return OrderRepositoryErrors.HaventFoundAnyOrder;

        await _genericRepository.Delete(orderFound);

        return true;
    }

    public async Task<ErrorOr<bool>> DeleteAnOrderByUserId(string userId, string orderId)
    {
        var userFound = await _userRepository.Get(userId);
        if (userFound.IsError)
            return userFound.Errors;
        
        var orderFound = await _genericRepository.Get(x => x.OrderId == orderId);
        if (orderFound == null!)
            return OrderRepositoryErrors.HaventFoundAnyOrderWithProvidedId;
        
        if(userFound.Value.Id != orderFound.UsersId) return false; 

        await _genericRepository.Delete(orderFound);

        return true;
    }

    public async Task<bool> Any(Expression<Func<Order, bool>> filter)
    {
        return await _genericRepository.Any(filter);
    }

    public async Task<(double? finalPrice, List<Item>? RemovedItems, List<Product> products)> CalculatingPrice(OrdersViewModel model)
    {
        double? finalPrice = 0;
        List<Item> removedItems = new();
        List<Product> products = new();
            
        foreach (var product in model.Products!)
        {
            var productFound = await _productsGenericRepository.Get(x => x.ProductName == product.Name);
            finalPrice += productFound.TotalProductPrice * product.Quantity;
                
            foreach (var item in product.Items!)
            {
                var itemFound = await _genericItemRepository.Get(x => x.ItemName!.ToLower() == item.Name!.ToLower());
                
                if (item.IsRemoved)
                {
                    removedItems.Add(itemFound);
                }

                if (item.AddItem)
                {
                    finalPrice += itemFound.ItemPrice;
                }
                
            }
            
            products.Add(productFound);
        }

        
        return (finalPrice, removedItems, products);
    }

    
}