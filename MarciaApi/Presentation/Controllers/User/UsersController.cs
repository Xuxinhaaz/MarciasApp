using ErrorOr;
using FluentValidation;
using MarciaApi.Infrastructure.Services.Auth.Authentication;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarciaApi.Presentation.Controllers.User;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IValidator<UserViewModel> _validator;
    private readonly IAuthenticationService _authenticationService;
    public UsersController(
        IValidator<UserViewModel> usersViewModelValidator, IAuthenticationService authenticationService)
    {
        _validator = usersViewModelValidator;
        _authenticationService = authenticationService;
    }
        
    [HttpPost("/Login")]
    public async Task<IActionResult> CreateAnUser([FromBody] UserViewModel viewModel)
    {
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
    
        var userModelDto = await _authenticationService.SignUpAsync(viewModel);
        if (userModelDto.IsError)
        {
            return new BadRequestObjectResult(new
            {
                userModelDto.Errors
            });
        }
        
        return new CreatedAtRouteResult("CreateAnUser", new
        {
            user = userModelDto.Value
        });
    }
    
}