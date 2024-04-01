using FluentValidation;
using MarciaApi.Application.Mapping.User;
using MarciaApi.Application.Services.Authentication;
using MarciaApi.Application.Services.Validator;
using MarciaApi.Application.Validator.Orders;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Repository.Items;
using MarciaApi.Infrastructure.Repository.Orders;
using MarciaApi.Infrastructure.Repository.Products;
using MarciaApi.Infrastructure.Repository.User;
using MarciaApi.Presentation.ViewModel.Orders;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        services.AddScoped<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
        services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
        services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
        services.AddScoped<IGenericRepository<Item>, GenericRepository<Item>>();
        
        services.AddTransient<IJwtService, JwtService>();
        
        services.AddAutoMapper(typeof(DomainToUserDto));
        
        
        return services;
    }
}