using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class UsersManagerController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserRepository _userRepository;
    public UsersManagerController(
        IAuthorizationService authorizationService, 
        IUserRepository userRepository)
    {
        _authorizationService = authorizationService;
        _userRepository = userRepository;
    }
    
    [HttpGet("/Manager/Users")] 
    public async Task<IActionResult> GetUsers([FromHeader] string authorization, [FromQuery] int pageNumber)
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
            users = await _userRepository.Get(pageNumber)
        });
    }
    
    [HttpDelete("/Manager/Users/{id}")]
    public async Task<IActionResult> DeleteAnUser([FromHeader] string authorization, [FromRoute] string id)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        var anyWithProvidedId = await _userRepository.Any(x => x.Id == id);
        if (!anyWithProvidedId)
        {
            return new NotFoundObjectResult(new
            {
                errors = new
                {
                    message = "Usuario não encontrado!"
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