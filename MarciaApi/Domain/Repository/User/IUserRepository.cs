using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Domain.Repository.User;

public interface IUserRepository
{
    Task<UserModel> Generate(UserViewModel model);
    Task<List<UserModelDto>> Get(int pageNumber);
    Task<UserModelDto> Get(string id);
    Task<OrderDto> Map(Order model);
    Task<List<OrderDto>> Map(List<Order> model);
    Task<bool> AnyUserWithSameEmailProvided(string email);
    Task<UserModel> FindByEmailAsync(string email);
    Task<bool> AnyWithProvidedId(string id);
    Task DeleteById(string id);
}   