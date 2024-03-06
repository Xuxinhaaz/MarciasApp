using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Application.Services.Authentication;

public interface IJwtService
{
    Task<string> Generate(UserViewModel model);
    Task<bool> Validate(string token);
}