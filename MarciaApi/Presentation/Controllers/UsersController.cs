using FluentValidation;
using MarciaApi.Application.Services.Email;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Infrastructure.Services.Authentication;
using MarciaApi.Infrastructure.Services.Email;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using IAuthenticationService = Microsoft.AspNetCore.Authentication.IAuthenticationService;

namespace MarciaApi.Presentation.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IValidator<UserViewModel> _usersViewModelValidator;
    private readonly MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService _authenticationService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    public UsersController(IValidator<UserViewModel> usersViewModelValidator, 
        MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService authenticationService, 
        IAuthorizationService authorizationService, 
        IUserRepository userRepository, AppDbContext context)
    {
        _usersViewModelValidator = usersViewModelValidator;
        _authenticationService = authenticationService;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
        _context = context;
    }

    [HttpGet("/Users")] 
    public async Task<IActionResult> GetUsers([FromHeader] string Authorization, [FromQuery] int pageNumber)
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
            users = await _userRepository.GetAll(pageNumber)
        });
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