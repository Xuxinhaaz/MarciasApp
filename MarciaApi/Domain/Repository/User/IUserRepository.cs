using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Domain.Repository.User;

public interface IUserRepository
{
    UserModel Generate(UserViewModel model);
    Task<List<UserModel>> GetAll(int pageNumber);
    Task<bool> AnyUserWithSameEmailProvided(string email);
    Task<UserModel> FindByEmailAsync(string email);
}   