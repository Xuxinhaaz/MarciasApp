using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Infrastructure.Services.Auth.Authorization;
using MarciaApi.Presentation.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ProductsManagerController
{
    private readonly IProductsRepository _productsRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IItemsRepository _itemsRepository;

    public ProductsManagerController(IProductsRepository productsRepository, IAuthorizationService authorizationService, IItemsRepository itemsRepository)
    {
        _productsRepository = productsRepository;
        _authorizationService = authorizationService;
        _itemsRepository = itemsRepository;
    }

    [HttpGet("/Manager/Products")]
    public async Task<IActionResult> Get(
        [FromHeader] string Authorization, 
        [FromQuery] int pageNumber)
    {
        var auth = await _authorizationService.AuthorizeManager(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "cannot access this endpoint"
                }
            });
        }

        var anyProducts = await _productsRepository.Any();
        if (!anyProducts)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Não há produtos registrados."
                }
            });
        }
        
        return new OkObjectResult(new
        {
            products = await _productsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Products/{id}")]

    public async Task<IActionResult> GetById(
        [FromHeader] string Authorization, 
        [FromRoute] string id)
    {
        var auth = await _authorizationService.AuthorizeManager(Authorization);
        if (!auth)  
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "Você não pode acessar esta página!"
                }
            });
        }

        var anyProducts = await _productsRepository.Any(x => x.ProdutId == id);
        if (!anyProducts)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Não há produtos registrados."
                }
            });
        }
        
        return new OkObjectResult(new
        {
            product = await _productsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Products")]
    public async Task<IActionResult> Post(
        [FromHeader] string Authorization, 
        [FromBody] ProductsViewModel viewModel)
    {
        var auth = await _authorizationService.AuthorizeManager(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "Você não pode acessar esta página!"
                }
            });
        }

        var anyProducts = await _productsRepository.Any(x => x.ProductName.ToLower() == viewModel.Name.ToLower()); 
        if (anyProducts)
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "já existe um produto com o mesmo nome cadastrado!"
                }
            });
        }

        foreach (var itemsName in viewModel.ItemsNames)
        {
            var anyItem = await _itemsRepository.Any(x => x.ItemName.ToLower() == itemsName.ToLower());
            if (!anyItem)
            {
                return new BadRequestObjectResult(new
                {
                    errors = new
                    {
                        message = $"o produto: {itemsName} não está cadastrado no sistema!"
                    }
                });
            }
        }

        var items = await _itemsRepository.GetByName(viewModel.ItemsNames);
        
        var newProduct = await _productsRepository.Generate(viewModel, items);

        return new CreatedAtRouteResult("Products Controller", new
        {
            Product = newProduct
        });
    }
}