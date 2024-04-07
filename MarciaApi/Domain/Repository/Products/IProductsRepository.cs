using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Domain.Repository.Products;

public interface IProductsRepository
{
    Task<List<ProductDto>> Get(int pageNumber);
    Task<ProductDto> Get(string id);
    Task<ProductDto> Generate(ProductsViewModel model);
}