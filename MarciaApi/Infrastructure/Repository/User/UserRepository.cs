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

    public UserRepository(IGenericRepository<UserModel> genericRepository, AppDbContext context)
    {
        _genericRepository = genericRepository;
        _context = context;
    }

    public UserModel Generate(UserViewModel model)
    {
        var newUser =  new UserModel()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            Orders = new List<Order>()
        };
        newUser.Roles = new List<string>();
        newUser.Roles.Add("User");

        if (model.Email == "joojjunu@gmail.com")
        {
            newUser.Roles.Add("Manager");
        }

        _genericRepository.Add(newUser);
        _genericRepository.SaveAll();

        return newUser;
    }

    public async Task<List<UserModel>> GetAll(int pageNumber)
    {
        return await _genericRepository.Get(pageNumber);
    }

    public async Task<bool> AnyUserWithSameEmailProvided(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<UserModel> FindByEmailAsync(string email)
    {
        return await _context.Users.FirstAsync(x => x.Email == email);
    }
}