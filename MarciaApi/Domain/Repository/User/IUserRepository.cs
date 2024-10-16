using System.Linq.Expressions;
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
    Task<bool> Any(Expression<Func<UserModel, bool>> filter);
    Task<UserModel> Get(Expression<Func<UserModel, bool>> filter, params Expression<Func<UserModel, object>>[] includes);

    Task Delete(UserModel model);
    Task Delete(
        Expression<Func<UserModel, bool>> filter = null, 
        params Expression<Func<UserModel, object>>[] includes);
}   