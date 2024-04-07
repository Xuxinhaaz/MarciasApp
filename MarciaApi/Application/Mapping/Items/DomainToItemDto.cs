using AutoMapper;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Items;

namespace MarciaApi.Application.Mapping.Items;

public class DomainToItemDto : Profile
{
    public DomainToItemDto()
    {
        CreateMap<Item, ItemDto>();
    }
}