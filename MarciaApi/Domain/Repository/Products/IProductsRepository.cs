using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Domain.Repository.Products;

public interface IProductsRepository
{
    Task<List<ProductDto>> Get(int pageNumber);
    Task<ProductDto> Get(string id);
    Task<ProductDto> Generate(ProductsViewModel model, List<Item> items);
    Task<bool> Any(Expression<Func<Product, bool>> filter);
    Task<bool> Any();
    Task<List<Product>> GetByName(List<string> ProductsNames);
}