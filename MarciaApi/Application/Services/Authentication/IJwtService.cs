using ErrorOr;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Application.Services.Authentication;

public interface IJwtService
{
    Task<ErrorOr<string>> Generate(UserModel model);
    Task<ErrorOr<string>> Validate(string token);
    Task<ErrorOr<string>> ValidateMangager(string token);
}