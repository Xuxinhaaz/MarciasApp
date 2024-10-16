using AutoMapper;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Orders;

namespace MarciaApi.Application.Mapping.Orders;

public class DomainToOrderDto : Profile
{
    public DomainToOrderDto()
    {
        CreateMap<Order, OrderDto>();
    }
}