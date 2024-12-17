using System.Linq.Expressions;
using ErrorOr;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.Errors.RepositoryErrors;
using MarciaApi.Presentation.ViewModel.Items;

namespace MarciaApi.Infrastructure.Repository.Items;

public class ItemsRepository : IItemsRepository
{
    private readonly IGenericRepository<Item, ItemDto> _genericRepository;

    public ItemsRepository(IGenericRepository<Item, ItemDto> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<List<ItemDto>> Get(int pageNumber)
    {
        List<Item> items = await _genericRepository.Get(pageNumber,
            m => m.Products!
            );
        List<ItemDto> itemDtos = await _genericRepository.Map(items);

        return itemDtos;
    }

    public async Task<ErrorOr<ItemDto>> Get(string id)
    {
        Item item = await _genericRepository.Get(
            m => m.ItemId == id,
            m => m.Products!);
        if (item == null)
            return ItemsRepositoryErrors.ThereIsntItemWithProvidedId;
        
        ItemDto dto = await _genericRepository.Map(item);
        
        return dto;
    }

    public async Task<Item> Generate(ItemsViewModel model)
    {
        Item newItem = new()
        {
            ItemId = Guid.NewGuid().ToString(),
            ItemName = model.ItemName,
            ItemPrice = model.ItemPrice
        };

        await _genericRepository.Add(newItem);
        await _genericRepository.SaveAll();

        return newItem;
    }

    public async Task<ErrorOr<List<Item>>> GetByName(List<string>? itemsNames)
    {
        var foundItems = new List<Item>();
        
        foreach (var item in itemsNames!)
        {
            foundItems.Add(
                await _genericRepository.Get(x => x.ItemName != null && x.ItemName.ToUpper() == item.ToUpper(),
                x => x.Products!));
        }

        if (foundItems.Count < 0)
            return ItemsRepositoryErrors.ThereIsntItemWithProvidedSameName;

        return foundItems;
    }

    public async Task<ItemDto> Map(Item model)
    {
        return await _genericRepository.Map(model);
    }

    public async Task<List<ItemDto>> Map(List<Item> model)
    {
        return await _genericRepository.Map(model);
    }

    public async Task Add(Item item)
    {
        await _genericRepository.Add(item);
    }

    public async Task<bool> Any(Expression<Func<Item, bool>> filter)
    {
        return await _genericRepository.Any(filter);
    }

    public async Task<bool> Any()
    {
        return await _genericRepository.Any();
    }
}