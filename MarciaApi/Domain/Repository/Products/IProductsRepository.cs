using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Domain.Repository.Products;

public interface IProductsRepository
{
    Task<List<Product>> Get(int pageNumber);
    Task<Product> Get(string id);
    Task<Product> Generate(ProductsViewModel model);
}