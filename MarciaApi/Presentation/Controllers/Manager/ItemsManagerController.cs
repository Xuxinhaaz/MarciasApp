using ErrorOr;
using FluentValidation;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Infrastructure.Services.Auth.Authorizarion;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.Items;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ItemsManagerController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IItemsRepository _itemsRepository;
    private readonly IValidator<ItemsViewModel> _validator;

    public ItemsManagerController(IItemsRepository itemsRepository, IAuthorizationService authorizationService, IValidator<ItemsViewModel> validator)
    {
        _itemsRepository = itemsRepository;
        _authorizationService = authorizationService;
        _validator = validator;
    }

    [HttpGet("/Manager/Items")]
    public async Task<IActionResult> Get([FromHeader] string authorization, [FromQuery] int pageNumber)
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
            items = await _itemsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Items/{id}")]
    public async Task<IActionResult> GetById([FromHeader] string authorization, [FromRoute] string id)
    {
        var auth = _authorizationService.AuthorizeManager(authorization);
        if (auth.IsError)
        {
            return new UnauthorizedObjectResult(new
            {
                auth.Errors
            });
        }

        if (!await _itemsRepository.Any(x => x.ItemId == id))
        {
            return new NotFoundObjectResult(new
            {
                Errors = ItemsRepositoryErrors.ThereIsntItemWithProvidedId
            });
        }
        
        return new OkObjectResult(new
        {
            item = await _itemsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Items")]
    public async Task<IActionResult> Post([FromHeader] string authorization, [FromBody] ItemsViewModel viewModel)
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

        var isThereAnyItemWithExistedName = await _itemsRepository.Any(x => x.ItemName == viewModel.ItemName);
        if (isThereAnyItemWithExistedName)
        {
            return new BadRequestObjectResult(new
            {
                Errors = ItemsRepositoryErrors.AlreadyExistsItemWithProvidedName
            });
        }
        
        var newItem = await _itemsRepository.Generate(viewModel);

        return new CreatedAtRouteResult("Items Controller", new
        {
            item = newItem
        });
    }
}