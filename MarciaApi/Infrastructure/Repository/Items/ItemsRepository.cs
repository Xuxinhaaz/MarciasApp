using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository;
using MarciaApi.Domain.Repository.Items;
using MarciaApi.Infrastructure.Data;
using MarciaApi.Presentation.ViewModel.Items;
using Microsoft.EntityFrameworkCore;

namespace MarciaApi.Infrastructure.Repository.Items;

public class ItemsRepository : IItemsRepository
{
    private readonly AppDbContext _context;
    private readonly IGenericRepository<Item> _genericRepository;

    public ItemsRepository(IGenericRepository<Item> genericRepository, AppDbContext context)
    {
        _genericRepository = genericRepository;
        _context = context;
    }

    public async Task<List<Item>> Get(int pageNumber)
    {
        return await _genericRepository.Get(pageNumber);
    }

    public async Task<Item> Get(string id)
    {
        return await _genericRepository.GetByID(id);
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
            foundItems.Add(await _context.Items.FirstAsync(m => m.ItemName == item));
        }

        return foundItems;
    }
}