using MarciaApi.Application.Services.Authentication;

namespace MarciaApi.Infrastructure.Services.Auth.Authorizarion;

public class AuthorizationService : IAuthorizationService
{
    private readonly IJwtService _jwtService;

    public AuthorizationService(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<bool> Authorize(string token)
    {
        return await _jwtService.Validate(token);
    }

    public async Task<bool> AuthorizeManager(string token)
    {
        return await _jwtService.ValidateMangager(token);
    }
}