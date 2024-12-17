using ErrorOr;
using MarciaApi.Application.Services.Authentication;

namespace MarciaApi.Infrastructure.Services.Auth.Authorizarion;

public class AuthorizationService : IAuthorizationService
{
    private readonly IJwtService _jwtService;

    public AuthorizationService(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public ErrorOr<string> Authorize(string token)
    {
        return _jwtService.Validate(token).Result;
    }

    public ErrorOr<string> AuthorizeManager(string token)
    {
        return _jwtService.ValidateMangager(token).Result;
    }
}