using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.RoleRepo;
using MarciaApi.Presentation.DTOs.Roles;

namespace MarciaApi.Infrastructure.Repository.RoleRepo;

public class RoleRepository : IRoleRepository
{
    private readonly IGenericRepository<Roles, RolesDto> _genericRepository;

    public RoleRepository(IGenericRepository<Roles, RolesDto> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task AddUser(UserModel model, Expression<Func<Roles, bool>> filter)
    {
        var roleFound = await _genericRepository.Get(filter);
        
        roleFound.UserModels.Add(model);
    }

    public async Task SaveAll()
    {
        await _genericRepository.SaveAll();
    }
}