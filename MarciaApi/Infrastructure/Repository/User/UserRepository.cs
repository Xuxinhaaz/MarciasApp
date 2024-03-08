using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Repository.User;

public class UserRepository : IUserRepository
{
    private readonly IGenericRepository<UserModel> _genericRepository;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepository(IGenericRepository<UserModel> genericRepository, AppDbContext context, IConfiguration configuration)
    {
        _genericRepository = genericRepository;
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserModel> Generate(UserViewModel model)
    {
        var newUser =  new UserModel()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            Orders = new List<Order>()
        };
        newUser.Roles = new List<string>();
        newUser.Roles.Add("User");

        if (model.Email == _configuration["ManagerEmail"])
        {
            newUser.Roles.Add("Manager");
        }

        await _genericRepository.Add(newUser);
        await _genericRepository.SaveAll();

        return newUser;
    }

    public async Task<List<UserModel>> GetAll(int pageNumber)
    {
        return await _genericRepository.Get(pageNumber);
    }

    public async Task<UserModel> GetById(string? id)
    {   
        return await _context.Users.FindAsync(id);
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