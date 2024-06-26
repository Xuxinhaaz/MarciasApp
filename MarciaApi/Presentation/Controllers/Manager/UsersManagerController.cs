using FluentValidation;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorization;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class UsersManagerController
{
    private readonly MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService _authenticationService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserRepository _userRepository;
    public UsersManagerController(
        MarciaApi.Infrastructure.Services.Authentication.IAuthenticationService authenticationService, 
        IAuthorizationService authorizationService, 
        IUserRepository userRepository)
    {
        _authenticationService = authenticationService;
        _authorizationService = authorizationService;
        _userRepository = userRepository;
    }
    
    [HttpGet("/Manager/Users")] 
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
            users = await _userRepository.Get(pageNumber)
        });
    }
    
    [HttpDelete("/Manager/Users/{id}")]
    public async Task<IActionResult> DeleteAnUser([FromHeader] string Authorization, [FromRoute] string id)
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

        await _userRepository.Delete(x => x.Id == id);

        return new OkObjectResult(new
        {
            message = "The user has been deleted!"
        });
    }
}