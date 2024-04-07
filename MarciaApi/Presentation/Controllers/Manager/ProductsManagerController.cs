using MarciaApi.Domain.Repository.Products;
using MarciaApi.Presentation.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ProductsManagerController
{
    private readonly IProductsRepository _productsRepository;

    public ProductsManagerController(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    [HttpGet("/Manager/Products")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber)
    {
        return new OkObjectResult(new
        {
            products = await _productsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Products/{id}")]

    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        return new OkObjectResult(new
        {
            product = await _productsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Products")]
    public async Task<IActionResult> Post([FromBody] ProductsViewModel viewModel)
    {
        var newProduct = await _productsRepository.Generate(viewModel);

        return new OkObjectResult(new
        {
            Product = newProduct
        });
    }
}