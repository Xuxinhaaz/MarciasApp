using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Domain.Repository.User;

public interface IUserRepository
{
    Task<UserModel> Generate(UserViewModel model);
    Task<List<UserModel>> GetAll(int pageNumber);
    Task<UserModel> GetById(string id);
    Task<bool> AnyUserWithSameEmailProvided(string email);
    Task<UserModel> FindByEmailAsync(string email);
    Task<bool> AnyWithProvidedId(string id);
    Task DeleteById(string id);
}   