using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.ViewModel.Items;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace MarciaApi.Infrastructure.Repository.Items;

public class ItemsRepository : IItemsRepository
{
    private readonly AppDbContext _context;
    private readonly IGenericRepository<Item, ItemDto> _genericRepository;

    public ItemsRepository(IGenericRepository<Item, ItemDto> genericRepository, AppDbContext context)
    {
        _genericRepository = genericRepository;
        _context = context;
    }

    public async Task<List<ItemDto>> Get(int pageNumber)
    {
        List<Item> items = await _genericRepository.Get(pageNumber,
            m => m.Products
            );
        List<ItemDto> itemDtos = await _genericRepository.Map(items);

        return itemDtos;
    }

    public async Task<ItemDto> Get(string id)
    {
        Item item = await _genericRepository.Get(
            m => m.ItemId == id,
            m => m.Products);
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

    public async Task<List<Item>> GetByName(List<string>? ItemsNames)
    {
        var foundItems = new List<Item>();
        
        foreach (var item in ItemsNames)
        {
            foundItems.Add(
                await _genericRepository.Get(x => x.ItemName == item,
                x => x.Products));
        }

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
}