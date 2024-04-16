using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Infrastructure.Repository.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly IGenericRepository<Product, ProductDto> _genericProductRepository;
    private readonly IItemsRepository _itemsRepository;

    public ProductsRepository(IGenericRepository<Product, ProductDto> genericProductRepository, IItemsRepository itemsRepository)
    {
        _genericProductRepository = genericProductRepository;
        _itemsRepository = itemsRepository;
    }

    public async Task<List<ProductDto>> Get(int pageNumber)
    {
        List<Product> products =  await _genericProductRepository.Get(pageNumber,
            m => m.Orders,
            m => m.Items,
            m => m.Sizes
            );
        List<ProductDto> dtos = await _genericProductRepository.Map(products);

        return dtos;
    }

    public async Task<ProductDto> Get(string id)
    { 
        Product product = await _genericProductRepository.GetByID(id, m => m.ProdutId == id, 
            m => m.Items,
            m => m.Sizes,
            m => m.Orders
            );
        ProductDto dto = await _genericProductRepository.Map(product);

        return dto;
    }

    public async Task<ProductDto> Generate(ProductsViewModel model)
    {
        var itemsNewProduct = await _itemsRepository.GetByName(model.ItemsNames);
        
        Product newProduct = new()
        {
            Items = itemsNewProduct,
            ProductDescription = model.Description,
            ProductName = model.Name,
            ProdutId = Guid.NewGuid().ToString(),
            TotalProductPrice = model.Price
        };
        
        foreach (var item in itemsNewProduct)
        {
            item.Products.Add(newProduct);
        }
        
        var dto = await _genericProductRepository.Map(newProduct);
        
        await _genericProductRepository.Add(newProduct);
        await _genericProductRepository.SaveAll();
        
        return dto;
    }

    

    public async Task<List<Product>> GetByName(List<string>? ProductsNames)
    {
        List<Product> products = new();

        foreach (var name in ProductsNames)
        {
            products.Add(await _genericProductRepository.GetByID(name, x => x.ProductName == name));
        }
        
        return products;
    }

    public async Task<bool> Any(Expression<Func<Product, bool>> filter)
    {
        return await _genericProductRepository.Any(filter);
    }
}