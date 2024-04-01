using FluentValidation;
using MarciaApi.Domain.Repository.Orders;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
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

    public OrdersController(IOrderRepository orderRepository, IAuthorizationService authorizationService, IValidator<OrdersViewModel> validator, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _authorizationService = authorizationService;
        _validator = validator;
        _userRepository = userRepository;
    }

    [HttpGet("/Orders")]
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
            Orders = await _orderRepository.Get(pageNumber)
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
        
        var validationReponse = _validator.Validate(viewModel);
        if (!validationReponse.IsValid)
        {
            var errors = new ModelStateDictionary();

            foreach (var error in validationReponse.Errors)
            {
                errors.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return new BadRequestObjectResult(new
            {
                errors
            });
        }

        var anyWithProvidedId = await _userRepository.AnyWithProvidedId(id);
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

        var newOrder = await _orderRepository.Generate(id, viewModel);

        var orderDto = await _userRepository.Map(newOrder);

        return new OkObjectResult(new
        {
            order = orderDto
        });
    }
}