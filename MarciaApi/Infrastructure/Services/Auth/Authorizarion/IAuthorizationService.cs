namespace MarciaApi.Infrastructure.Services.Auth.Authorizarion;

public interface IAuthorizationService
{
    Task<bool> Authorize(string token);
    Task<bool> AuthorizeManager(string token);
}