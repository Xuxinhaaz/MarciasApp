using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Application.Services.Authentication;

public interface IJwtService
{
    Task<string> Generate(UserModel model);
    Task<bool> Validate(string token);
    Task<bool> ValidateMangager(string token);
}