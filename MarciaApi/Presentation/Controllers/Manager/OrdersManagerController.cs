using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class OrdersManagerController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrderRepository _orderRepository;

    public OrdersManagerController(IOrderRepository orderRepository, IAuthorizationService authorizationService)
    {
        _orderRepository = orderRepository;
        _authorizationService = authorizationService;
    }
    
    [HttpGet("/Manager/Orders")]
    public async Task<IActionResult> GetOrders([FromHeader] string authorization, [FromQuery] int pageNumber)
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
            Orders = await _orderRepository.Get(pageNumber, null)
        });
    }

    [HttpDelete("/Manager/Orders/{id}")]
    public async Task<IActionResult> DeleteAnOrder([FromHeader] string authorization, [FromRoute] string id)
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