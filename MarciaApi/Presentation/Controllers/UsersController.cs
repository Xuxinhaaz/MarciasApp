using FluentValidation;
using MarciaApi.Application.Services.Email;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Authentication;
using MarciaApi.Infrastructure.Services.Email;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IAuthenticationService = Microsoft.AspNetCore.Authentication.IAuthenticationService;

namespace MarciaApi.Presentation.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IValidator<UserViewModel> _usersViewModelValidator;
    private readonly MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService _authenticationService;

    public UsersController( 
        IValidator<UserViewModel> usersViewModelValidator, 
        MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService authenticationService)
    {
        _usersViewModelValidator = usersViewModelValidator;
        _authenticationService = authenticationService;
    }

    [HttpGet("/")]
    public async Task<IActionResult> Get()
    {
        return new OkObjectResult("Ok");
    }
    
    [HttpPost("/Users")]
    public async Task<IActionResult> CreateAnUser([FromBody] UserViewModel viewModel)
    {
        var responseAuth = _usersViewModelValidator.Validate(viewModel);
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
        
        return new OkObjectResult(new
        {
            user = userModelDto
        });
    }
}