using System.Linq.Expressions;
using AutoMapper;
using ErrorOr;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.RoleRepo;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Presentation.DTOs.Roles;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Repository.User;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<UserModel, UserModelDto> _genericRepository;
    private readonly IGenericRepository<Roles, RolesDto> _genericRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;

    public UserRepository(
        IGenericRepository<UserModel, UserModelDto> genericRepository, 
        IGenericRepository<Roles, RolesDto> genericRoleRepository,
        IConfiguration configuration, 
        IMapper mapper, IRoleRepository roleRepository)
    {
        _genericRoleRepository = genericRoleRepository;
        _genericRepository = genericRepository;
        _configuration = configuration;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<UserModel>> Generate(UserViewModel model)
    {
        var clientRole = await _genericRoleRepository.Get(x => x.Role == "Client");
        
        var newUser =  new UserModel()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
        };

        newUser.Roles.Add(clientRole);

        if (model.Email == _configuration["ManagerEmail"])
        {
            newUser.Roles.Add(await _genericRoleRepository.Get(x => x.Role == "Manager"));
            await _roleRepository.AddUser(newUser, x => x.Role == "Manager");
        }

        await _roleRepository.AddUser(newUser, x => x.Role == "Client");
        
        await _genericRepository.Add(newUser);
        await _genericRepository.SaveAll();

        return newUser;
    }

    public async Task<List<UserModelDto>> Get(int pageNumber)
    {
        List<UserModel> userModels = await _genericRepository.Get(pageNumber);
        List<UserModelDto> dtos = await _genericRepository.Map(userModels);

        return dtos;
    }

    public async Task<ErrorOr<UserModelDto>> Get(string? id)
    {   
        UserModel userModel = await _genericRepository.Get(m => m.Id == id);
        if (userModel == null)
            return UserRepositoryErrors.HaventFoundAnyUserWithProvidedId;
        
        UserModelDto dto = await _genericRepository.Map(userModel);

        return dto;
    }

    public async Task<bool> Any(Expression<Func<UserModel, bool>> filter)
    {
        return await _genericRepository.Any(filter);
    }

    public async Task<UserModel> Get(Expression<Func<UserModel, bool>> filter, Expression<Func<UserModel, object>>[] includes)
    {
        return await _genericRepository.Get(filter, includes);
    }

    public async Task Delete(
        Expression<Func<UserModel, bool>> filter = null, 
        params Expression<Func<UserModel, object>>[] includes)
    {
        await _genericRepository.Delete(filter, includes);
    }

    public async Task Delete(UserModel model)
    {
        await _genericRepository.Delete(model);
        await _genericRepository.SaveAll();
    }
}