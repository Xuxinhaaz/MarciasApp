using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Repository.User;

public class UserRepository : IUserRepository
{
    
    public UserModel Generate(UserViewModel model)
    {
        return new UserModel()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email
        };
    }
    
}