using MarciaApi.Domain.Repository.Items;
using MarciaApi.Infrastructure.Services.Auth.Authorization;
using MarciaApi.Presentation.ViewModel.Items;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ItemsManagerController
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IItemsRepository _itemsRepository;

    public ItemsManagerController(IItemsRepository itemsRepository, IAuthorizationService authorizationService)
    {
        _itemsRepository = itemsRepository;
        _authorizationService = authorizationService;
    }

    [HttpGet("/Manager/Items")]
    public async Task<IActionResult> Get([FromHeader] string Authorization, [FromQuery] int pageNumber)
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
        
        return new OkObjectResult(new
        {
            items = await _itemsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Items/{id}")]
    public async Task<IActionResult> GetById([FromHeader] string Authorization, [FromRoute] string id)
    {
        var auth = await _authorizationService.AuthorizeManager(Authorization);
        if (!auth)
        {
            return new UnauthorizedObjectResult(new
            {
                errors = new
                {
                    message = "você não pode acessar este endpoint!"
                }
            });
        }

        if (!await _itemsRepository.Any(x => x.ItemId == id))
        {
            return new BadRequestObjectResult(new
            {
                errors = new
                {
                    message = "Este item não está registrado no sistema!"
                }
            });
        }
        
        return new OkObjectResult(new
        {
            item = await _itemsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Items")]
    public async Task<IActionResult> Post([FromHeader] string Authorization, [FromBody] ItemsViewModel viewModel)
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
        
        var newItem = await _itemsRepository.Generate(viewModel);

        return new CreatedAtRouteResult("Items Controller", new
        {
            item = newItem
        });
    }
}