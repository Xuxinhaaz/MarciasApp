using MarciaApi.Domain.Repository.Items;
using MarciaApi.Presentation.ViewModel.Items;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers.Manager;

[ApiController]
public class ItemsManagerController
{
    private readonly IItemsRepository _itemsRepository;

    public ItemsManagerController(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    [HttpGet("/Manager/Items")]
    public async Task<IActionResult> Get([FromQuery] int pageNumber)
    {
        return new OkObjectResult(new
        {
            items = await _itemsRepository.Get(pageNumber)
        });
    }
    
    [HttpGet("/Manager/Items/{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        return new OkObjectResult(new
        {
            item = await _itemsRepository.Get(id)
        });
    }

    [HttpPost("/Manager/Items")]
    public async Task<IActionResult> Post([FromBody] ItemsViewModel viewModel)
    {
        var newItem = await _itemsRepository.Generate(viewModel);

        return new OkObjectResult(new
        {
            item = newItem
        });
    }
}