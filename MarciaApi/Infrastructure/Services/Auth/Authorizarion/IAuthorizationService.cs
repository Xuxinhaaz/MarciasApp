namespace MarciaApi.Infrastructure.Services.Auth.Authorization;

public interface IAuthorizationService
{
    Task<bool> Authorize(string token);
    Task<bool> AuthorizeManager(string token);
}