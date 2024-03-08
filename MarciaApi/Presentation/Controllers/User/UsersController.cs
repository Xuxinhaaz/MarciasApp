using FluentValidation;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MarciaApi.Presentation.Controllers.User;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IValidator<UserViewModel> _usersViewModelValidator;
    private readonly MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService _authenticationService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserRepository _userRepository;
    public UsersController(IValidator<UserViewModel> usersViewModelValidator, 
        MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService authenticationService, 
        IAuthorizationService authorizationService, 
        IUserRepository userRepository)
    {
        _usersViewModelValidator = usersViewModelValidator;
        _authenticationService = authenticationService;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
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
        
        return new CreatedAtRouteResult("DeleteAnUser", new
        {
            user = userModelDto
        });
    }
    
}