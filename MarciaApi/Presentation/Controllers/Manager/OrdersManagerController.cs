using FluentValidation;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorization;
using MarciaApi.Presentation.ViewModel.Orders;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class OrdersManagerController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<OrdersViewModel> _validator;
    private readonly IUserRepository _userRepository;

    public OrdersManagerController(IOrderRepository orderRepository, IAuthorizationService authorizationService, IValidator<OrdersViewModel> validator, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _authorizationService = authorizationService;
        _validator = validator;
        _userRepository = userRepository;
    }
    
    [HttpGet("/Manager/Orders")]
    public async Task<IActionResult> GetOrders([FromHeader] string Authorization, [FromQuery] int pageNumber)
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

    [HttpDelete("/Manager/Orders/{id}")]
    public async Task<IActionResult> DeleteAnOrder([FromHeader] string Authorization, [FromRoute] string id)
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

        var anyOrderWithProvidedId = await _orderRepository.Any(x => x.OrderId == id);
        if (!anyOrderWithProvidedId)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Não há este pedido registrado no sistema!"
                }
            });
        }

        await _orderRepository.Delete(x => x.OrderId == id);

        return new OkObjectResult(new
        {
            message = "pedido deletado!"
        });
    }
}