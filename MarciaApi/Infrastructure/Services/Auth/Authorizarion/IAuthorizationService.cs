using ErrorOr;

namespace MarciaApi.Infrastructure.Services.Auth.Authorizarion;

public interface IAuthorizationService
{
    ErrorOr<string> Authorize(string token);
    ErrorOr<string> AuthorizeManager(string token);
}