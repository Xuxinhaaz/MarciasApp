using AutoMapper;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.DTOs.Orders;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Repository.User;

public class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<UserModel, UserModelDto> _genericRepository;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepository(IGenericRepository<UserModel, UserModelDto> genericRepository, AppDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _genericRepository = genericRepository;
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<UserModel> Generate(UserViewModel model)
    {
        var ClientRole = await _context.Roles.FirstAsync(x => x.Role == "Client");
        
        var newUser =  new UserModel()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
        };
       
        newUser.Roles.Add(ClientRole);

        if (model.Email == _configuration["ManagerEmail"])
        {
            newUser.Roles.Add(await _context.Roles.FirstAsync(x => x.Role == "Manager"));
        }
        
        

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

    public async Task<OrderDto> Map(Order model)
    {
        return _mapper.Map<OrderDto>(model);
    }

    public async Task<List<OrderDto>> Map(List<Order> model)
    {
        return _mapper.Map<List<OrderDto>>(model);
    }

    public async Task<UserModelDto> Get(string? id)
    {   
        UserModel userModel = await _genericRepository.GetByID(id, m => m.Id == id);
        UserModelDto dto = await _genericRepository.Map(userModel);

        return dto;
    }

    public async Task<bool> AnyWithProvidedId(string id)
    {
        return await _context.Users.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> AnyUserWithSameEmailProvided(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<UserModel> FindByEmailAsync(string email)
    {
        return await _context.Users.FirstAsync(x => x.Email == email);
    }

    public async Task DeleteById(string id)
    {
        var userFound = await _context.Users
            .Include(x => x.Orders)
            .FirstAsync(x => x.Id == id);

        await _genericRepository.Delete(userFound);
        await _genericRepository.SaveAll();
    }
}