using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Services.Authentication;

public interface IAuthenticationService
{
     Task<UserModelDto> SignUpAsync(UserViewModel model);
}