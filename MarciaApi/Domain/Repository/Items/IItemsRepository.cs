using System.Linq.Expressions;
using MarciaApi.Domain.Models;
using MarciaApi.Presentation.DTOs.Items;
using MarciaApi.Presentation.ViewModel.Items;

namespace MarciaApi.Domain.Repository.Items;

public interface IItemsRepository
{
    Task<List<ItemDto>> Get(int pageNumber);
    Task<ItemDto> Get(string id);
    Task<Item> Generate(ItemsViewModel model);
    Task<List<Item>> GetByName(List<string>? ItemsNames);
    Task Add(Item item);
    Task<bool> Any(Expression<Func<Item, bool>> filter);
    Task<bool> Any();
}