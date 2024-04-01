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
            products = _productsRepository.Get(pageNumber)
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