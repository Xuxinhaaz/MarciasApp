using ErrorOr;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Services.Auth.Authentication;

public interface IAuthenticationService
{
     Task<ErrorOr<UserModelDto>> SignUpAsync(UserViewModel model);
}