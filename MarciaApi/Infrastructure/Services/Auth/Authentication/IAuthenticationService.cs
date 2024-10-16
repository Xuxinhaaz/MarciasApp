using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Services.Auth.Authentication;

public interface IAuthenticationService
{
     Task<UserModelDto> SignUpAsync(UserViewModel model);
}