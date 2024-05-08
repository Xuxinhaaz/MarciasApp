using System.Linq.Expressions;
using MarciaApi.Domain.Models;

namespace MarciaApi.Domain.Repository.RoleRepo;

public interface IRoleRepository
{
    Task AddUser(UserModel model, Expression<Func<Roles, bool>> filter);
    Task SaveAll();
}