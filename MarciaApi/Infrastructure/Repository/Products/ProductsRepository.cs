using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Infrastructure.Repository.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly IGenericRepository<Product> _genericProductRepository;
    private readonly IItemsRepository _itemsRepository;

    public ProductsRepository(IGenericRepository<Product> genericProductRepository, IItemsRepository itemsRepository)
    {
        _genericProductRepository = genericProductRepository;
        _itemsRepository = itemsRepository;
    }

    public async Task<List<Product>> Get(int pageNumber)
    {
        return await _genericProductRepository.Get(pageNumber);
    }

    public async Task<Product> Get(string id)
    { 
        return await _genericProductRepository.GetByID(id);
    }

    public async Task<Product> Generate(ProductsViewModel model)
    {
        var newProduct = new Product()
        {
            Sizes = model.Sizes,
            Items = await _itemsRepository.GetByName(model.ItemsNames),
            ProductDescription = model.Description,
            ProductName = model.Name,
            ProdutId = Guid.NewGuid().ToString(),
            TotalProductPrice = model.Price
        };
        
        await _genericProductRepository.Add(newProduct);
        await _genericProductRepository.SaveAll();
        
        return newProduct;
    }
}