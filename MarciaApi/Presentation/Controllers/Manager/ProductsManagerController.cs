using ErrorOr;
using FluentValidation;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Domain.Repository.Products;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ProductsManagerController
{
    private readonly IProductsRepository _productsRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IItemsRepository _itemsRepository;
    private readonly IValidator<ProductsViewModel> _validator;

    public ProductsManagerController(IProductsRepository productsRepository, IAuthorizationService authorizationService, IItemsRepository itemsRepository, IValidator<ProductsViewModel> validator)
    {
        _productsRepository = productsRepository;
        _authorizationService = authorizationService;
        _itemsRepository = itemsRepository;
        _validator = validator;
    }

    [HttpGet("/Manager/Products")]
    public async Task<IActionResult> Get(
        [FromHeader] string authorization, 
        [FromQuery] int pageNumber)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }
        
        return new OkObjectResult(new
        {
            products = await _productsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Products/{id}")]

    public async Task<IActionResult> GetById(
        [FromHeader] string authorization, 
        [FromRoute] string id)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        var anyProducts = await _productsRepository.Any(x => x.ProdutId == id);
        if (!anyProducts)
        {
            return new BadRequestObjectResult(new
            {
                Errors = ProductRepositoryErrors.HaventFoundProductWithProvidedId
            });
        }
        
        return new OkObjectResult(new
        {
            product = await _productsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Products")]
    public async Task<IActionResult> Post(
        [FromHeader] string authorization, 
        [FromForm] ProductsViewModel viewModel)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }
        
        var validationResponse = _validator.Validate(viewModel);
        if (!validationResponse.IsValid)
        {
            var errors = validationResponse.Errors
                .Select(x => Error.Validation(x.PropertyName, x.ErrorMessage))
                .ToList();

            return new BadRequestObjectResult(new
            {
                errors
            });
        }

        var anyProducts = await _productsRepository.Any(x => x.ProductName!.ToLower() == viewModel.Name!.ToLower()); 
        if (anyProducts)
        {
            return new BadRequestObjectResult(new
            {
                errors = ProductRepositoryErrors.ThereIsAnExistingProductWithSameName
            });
        }

        foreach (var itemsName in viewModel.ItemsNames!)
        {
            var anyItem = await _itemsRepository.Any(x => x.ItemName!.ToLower() == itemsName.ToLower());
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
        if (items.IsError)
        {
            return new NotFoundObjectResult(new
            {
                items.Errors
            });
        }
        
        var newProduct = await _productsRepository.Generate(viewModel, items.Value);
        if (newProduct.IsError)
            return new BadRequestObjectResult(new
            {
                newProduct.Errors
            });

        return new CreatedAtRouteResult("Products Controller", new
        {
            Product = newProduct.Value
        });
    }
}