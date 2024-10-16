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
    private readonly IValidator<UserViewModel> _usersViewModelValidator;
    private readonly IAuthenticationService _authenticationService;
    public UsersController(
        IValidator<UserViewModel> usersViewModelValidator, IAuthenticationService authenticationService)
    {
        _usersViewModelValidator = usersViewModelValidator;
        _authenticationService = authenticationService;
    }
        
    [HttpPost("/Login")]
    public async Task<IActionResult> CreateAnUser([FromBody] UserViewModel viewModel)
    {
        var responseAuth = await _usersViewModelValidator.ValidateAsync(viewModel);
        if (!responseAuth.IsValid)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in responseAuth.Errors)
            {
                modelStateDictionary.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return new BadRequestObjectResult(new
            {
                errors = modelStateDictionary
            });
        }

        UserModelDto userModelDto = await _authenticationService.SignUpAsync(viewModel);
        
        return new CreatedAtRouteResult("DeleteAnUser", new
        {
            user = userModelDto
        });
    }
    
}