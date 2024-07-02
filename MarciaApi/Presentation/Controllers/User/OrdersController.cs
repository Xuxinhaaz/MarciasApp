using FluentValidation;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorization;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarciaApi.Presentation.Controllers;

[ApiController]
public class OrdersController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<OrdersViewModel> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IGenericRepository<Item, ItemDto> _genericItemRepository;

    public OrdersController(IOrderRepository orderRepository, 
    IAuthorizationService authorizationService, 
    IValidator<OrdersViewModel> validator,  
    IUserRepository userRepository, IProductsRepository productsRepository, IGenericRepository<Item, ItemDto> genericItemRepository)
    {
        _orderRepository = orderRepository;
        _authorizationService = authorizationService;
        _validator = validator;
        _userRepository = userRepository;
        _productsRepository = productsRepository;
        _genericItemRepository = genericItemRepository;
    }

    [HttpGet("/Orders/{id}")]
    public async Task<IActionResult> GetOrders([FromHeader] string authorization, 
        [FromRoute] string id,
        [FromQuery] int pageNumber)
    {
        var auth = await _authorizationService.AuthorizeManager(authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "cannot access this endpoint"
                }
            });
        }

        return new OkObjectResult(new
        {
            Orders = await _orderRepository.Get(pageNumber, x => x.UsersId == id, x => x.Products)
        });
    }
    
    [HttpGet("/Orders")]
    public async Task<IActionResult> GetOrders([FromHeader] string Authorization, 
        [FromQuery] int pageNumber)
    {
        var auth = await _authorizationService.AuthorizeManager(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "cannot access this endpoint"
                }
            });
        }

        return new OkObjectResult(new
        {
            Orders = await _orderRepository.Get(pageNumber, null)
        });
    }

    [HttpPost("/Orders/{id}")]
    public async Task<IActionResult> PostAnOrder([FromHeader] string Authorization, 
        [FromRoute] string id,
        [FromBody] OrdersViewModel viewModel)
    {
        var auth = await _authorizationService.Authorize(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "cannot access this endpoint"
                }
            });
        }
        
        var validationResponse = _validator.Validate(viewModel);
        if (!validationResponse.IsValid)
        {
            var errors = new ModelStateDictionary();

            foreach (var error in validationResponse.Errors)
            {
                errors.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return new BadRequestObjectResult(new
            {
                errors
            });
        }

        var anyWithProvidedId = await _userRepository.Any(x => x.Id == id);
        if (!anyWithProvidedId)
        {
            return new NotFoundObjectResult(new
            {
                errors = new
                {
                    message = "User not found!"
                }
            });
        }

        foreach (var product in viewModel.Products)
        {
            if (!await _productsRepository.Any(x => x.ProductName == product.Name))
            {
                return new BadRequestObjectResult(new
                {
                    errors = new
                    {
                        message = $"Não há produtos com nome: {product.Name} cadastrado no sistema"
                    }
                });
            }

            foreach (var item in product.Items)
            {
                if (!await _genericItemRepository.Any(x => x.ItemName == item.Name))
                {
                    return new BadRequestObjectResult(new
                    {
                        errors = new
                        {
                            message = $"Não há nenhum item com nome: {item.Name} cadastrado no sistema"
                        }
                    });
                }
            }
        }
        
        var newOrder = await _orderRepository.Generate(id, viewModel);
        if (newOrder is null)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Um erro desconhecido aconteceu!"
                }
            });
        }

        return new OkObjectResult(new
        {
            order = newOrder
        });
    }

    [HttpDelete("/Orders/{id}")]
    public async Task<IActionResult> DeleteAnOrder([FromHeader] string Authorization, [FromRoute] string id, [FromQuery] string orderId)
    {
        var auth = await _authorizationService.Authorize(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "voce não esta autenticado"
                }
            });
        }

        var anyUserWithProvidedId = await _orderRepository.Any(x => x.UsersId == id);
        if (!anyUserWithProvidedId)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Voce não tem nenhum pedido"
                }
            });
        }

        var doesOrderExists = await _orderRepository.Any(x => x.OrderId == orderId);
        if(!doesOrderExists) return new BadRequestObjectResult(new
        {
            errors = new
            {
                message = "Este pedido não existe!"
            }
        });

        if(!await _orderRepository.DeleteAnOrderByUserId(id, orderId)) 
        {
            return new BadRequestObjectResult(new {
                errors = new 
                {
                    message = "Não foi possível deletar o pedido"
                }
            });
        }

        return new OkObjectResult(new
        {
            message = "pedido deletado!"
        });
    }
}