using AutoMapper;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.User;

namespace MarciaApi.Application.Mapping.User;

public class DomainToUserDto : Profile
{
    public DomainToUserDto()
    {
        CreateMap<UserModel, UserModelDto>();
    }
}