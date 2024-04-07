using AutoMapper;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Products;

namespace MarciaApi.Application.Mapping.Products;

public class DomainToProductDto : Profile
{
    public DomainToProductDto()
    {
        CreateMap<Product, ProductDto>();
    }
}