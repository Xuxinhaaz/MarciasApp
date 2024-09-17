using FluentValidation;
using MarciaApi.Application.Mapping.Items;
using MarciaApi.Application.Mapping.Orders;
using MarciaApi.Application.Mapping.Products;
using MarciaApi.Application.Mapping.Roles;
using MarciaApi.Application.Mapping.User;
using MarciaApi.Application.Services.Authentication;
using MarciaApi.Application.Services.Validator;
using MarciaApi.Application.Validator.Orders;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Domain.Repository.RoleRepo;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Repository.Items;
using MarciaApi.Infrastructure.Repository.Orders;
using MarciaApi.Infrastructure.Repository.Products;
using MarciaApi.Infrastructure.Repository.RoleRepo;
using MarciaApi.Infrastructure.Repository.User;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.DTOs.Roles;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.Orders;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserViewModel>, UserViewModelValidator>();
        services.AddScoped<IValidator<OrdersViewModel>, OrderViewModelValidator>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<IItemsRepository, ItemsRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IGenericRepository<UserModel, UserModelDto>, GenericRepository<UserModel, UserModelDto>>();
        services.AddScoped<IGenericRepository<Order, OrderDto>, GenericRepository<Order, OrderDto>>();
        services.AddScoped<IGenericRepository<Product, ProductDto>, GenericRepository<Product, ProductDto>>();
        services.AddScoped<IGenericRepository<Item, ItemDto>, GenericRepository<Item, ItemDto>>();
        services.AddScoped<IGenericRepository<Roles, RolesDto>, GenericRepository<Roles, RolesDto>>();
        
        services.AddTransient<IJwtService, JwtService>();
        
        services.AddAutoMapper(typeof(DomainToUserDto));
        services.AddAutoMapper(typeof(DomainToOrderDto));
        services.AddAutoMapper(typeof(DomainToProductDto));
        services.AddAutoMapper(typeof(DomainToItemDto));
        services.AddAutoMapper(typeof(DomainToRoleDto));
        
        return services;
    }
}