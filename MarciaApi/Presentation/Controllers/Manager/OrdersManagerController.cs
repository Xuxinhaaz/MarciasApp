using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Presentation.Errors.RepositoryErrors;
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
        var auth = _authorizationService.AuthorizeManager(authorization);
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

    [HttpDelete("/Manager/Orders/{id}")]
    public async Task<IActionResult> DeleteAnOrder([FromHeader] string authorization, [FromRoute] string id)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        var anyOrderWithProvidedId = await _orderRepository.Any(x => x.OrderId == id);
        if (!anyOrderWithProvidedId)
        {
            return new NotFoundObjectResult(new
            {
                Errors = OrderRepositoryErrors.HaventFoundAnyOrderWithProvidedId
            });
        }

        await _orderRepository.Delete(x => x.OrderId == id);

        return new OkObjectResult(new
        {
            description = "pedido deletado!"
        });
    }
}