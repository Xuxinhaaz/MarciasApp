using AutoMapper;
using MarciaApi.Presentation.DTOs.Roles;

namespace MarciaApi.Application.Mapping.Roles
{
    public class DomainToRoleDto : Profile
    {
        public DomainToRoleDto()
        {
            CreateMap<Domain.Models.Roles, RolesDto>();
        }
    }
}