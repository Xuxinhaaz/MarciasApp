using System.Linq.Expressions;
using Amazon.S3.Model;
using MarciaApi.Domain.Data.Cloud;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Presentation.DTOs.Products;
using MarciaApi.Presentation.ViewModel.Products;

namespace MarciaApi.Infrastructure.Repository.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly IGenericRepository<Product, ProductDto> _genericProductRepository;
    private readonly ICloudflare _cloudflare;

    public ProductsRepository(
        IGenericRepository<Product, ProductDto> genericProductRepository, ICloudflare cloudflare)
    {
        _genericProductRepository = genericProductRepository;
        _cloudflare = cloudflare;
    }

    public async Task<List<ProductDto>> Get(int pageNumber)
    {
        List<Product> products =  await _genericProductRepository.Get(pageNumber,
            m => m.Orders!,
            m => m.Items!,
            m => m.Sizes!
            );
        List<ProductDto> dtos = await _genericProductRepository.Map(products);

        return dtos;
    }

    public async Task<ProductDto> Get(string id)
    { 
        Product product = await _genericProductRepository.Get(
            m => m.ProdutId == id, 
            m => m.Items!,
            m => m.Sizes!,
            m => m.Orders!);
        ProductDto dto = await _genericProductRepository.Map(product);

        return dto;
    }

    public async Task<ProductDto> Generate(ProductsViewModel model, List<Item> items)
    {
        Product newProduct = new()
        {
            Items = items,
            ProductDescription = model.Description,
            ProductName = model.Name,
            ProdutId = Guid.NewGuid().ToString(),
            TotalProductPrice = model.Price
        };
        
        foreach (var item in items)
        {
            item.Products?.Add(newProduct);
        }
        
        var dto = await _genericProductRepository.Map(newProduct);
        
        await _genericProductRepository.Add(newProduct);
        await _genericProductRepository.SaveAll();

        await _cloudflare.AddFile(model.ProductPhoto!);

        return dto;
    }

    public async Task<List<Product>> GetByName(List<string>? productsNames)
    {
        List<Product> products = new();

        foreach (var name in productsNames!)
        {
            products.Add(await _genericProductRepository.Get(x => x.ProductName == name));
        }
        
        return products;
    }

    public async Task<bool> Any(Expression<Func<Product, bool>> filter)
    {
        return await _genericProductRepository.Any(filter);
    }
    
    public async Task<bool> Any()
    {
        return await _genericProductRepository.Any();
    }
}