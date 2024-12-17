using ErrorOr;
using FluentValidation;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.User;

[ApiController]
public class OrdersController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<OrdersViewModel> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IItemsRepository _itemsRepository;
    public OrdersController(IOrderRepository orderRepository, 
    IAuthorizationService authorizationService, 
    IValidator<OrdersViewModel> validator,  
    IUserRepository userRepository, IProductsRepository productsRepository, IItemsRepository itemsRepository)
    {
        _orderRepository = orderRepository;
        _authorizationService = authorizationService;
        _validator = validator;
        _userRepository = userRepository;
        _productsRepository = productsRepository;
        _itemsRepository = itemsRepository;
    }

    [HttpGet("/Orders/{id}")]
    public async Task<IActionResult> GetOrders([FromHeader] string authorization, 
        [FromRoute] string id,
        [FromQuery] int pageNumber)
    {
        var auth = _authorizationService.Authorize(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        return new OkObjectResult(new
        {
            Orders = await _orderRepository.Get(pageNumber, x => x.UsersId == id, x => x.Products!)
        });
    }
    
    [HttpGet("/Orders")]
    public async Task<IActionResult> GetOrders([FromHeader] string authorization, 
        [FromQuery] int pageNumber)
    {
        var auth = _authorizationService.Authorize(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        return new OkObjectResult(new
        {
            Orders = await _orderRepository.Get(pageNumber, null)
        });
    }

    [HttpPost("/Orders/{id}")]
    public async Task<IActionResult> PostAnOrder([FromHeader] string authorization, 
        [FromRoute] string id,
        [FromBody] OrdersViewModel viewModel)
    {
        var auth = _authorizationService.Authorize(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        
        var validationResponse = _validator.Validate(viewModel);
        if (!validationResponse.IsValid)
        {
            var errors = validationResponse.Errors
                .Select(x => Error.Validation(x.PropertyName, x.ErrorMessage))
                .ToList();

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

        foreach (var product in viewModel.Products!)
        {
            var productWithSameName = await _productsRepository.Any(x => x.ProductName!.ToLower() == product.Name!.ToLower());
            
            if (!productWithSameName)
            {
                return new BadRequestObjectResult(new
                {
                    Errors = ProductRepositoryErrors.ThereIsntAnExistingProductWithSameName
                });
            }

            foreach (var item in product.Items!)
            {
                if (!await _itemsRepository.Any(x => x.ItemName == item.Name))
                {
                    return new BadRequestObjectResult(new
                    {
                        Errors = ItemsRepositoryErrors.AlreadyExistsItemWithProvidedName
                    });
                }
            }
        }
        
        var newOrder = await _orderRepository.Generate(id, viewModel);
        if (newOrder.IsError)
        {
            return new BadRequestObjectResult(new
            {
                newOrder.Errors
            });
        }
        
        return new OkObjectResult(new
        {
            order = newOrder
        });
    }

    [HttpDelete("/Orders/{id}")]
    public async Task<IActionResult> DeleteAnOrder([FromHeader] string authorization, [FromRoute] string id, [FromQuery] string orderId)
    {
        var auth = _authorizationService.Authorize(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        var anyUserWithProvidedId = await _orderRepository.Any(x => x.UsersId == id);
        if (!anyUserWithProvidedId)
        {
            return new NotFoundObjectResult(new
            {
                UserRepositoryErrors.HaventFoundAnyUserWithProvidedId
            });
        }

        var doesOrderExists = await _orderRepository.Any(x => x.OrderId == orderId);
        if(!doesOrderExists) return new NotFoundObjectResult(new
        {
            Errros = OrderRepositoryErrors.HaventFoundAnyOrderWithProvidedId
        });

        var deleteAnOrderByUserId = await _orderRepository.DeleteAnOrderByUserId(id, orderId);
        if(deleteAnOrderByUserId.IsError) 
        {
            return new BadRequestObjectResult(new {
                deleteAnOrderByUserId.Errors
            });
        }

        return new OkObjectResult(new
        {
            message = "pedido deletado!"
        });
    }

    
}