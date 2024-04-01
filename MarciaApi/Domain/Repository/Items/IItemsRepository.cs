using MarciaApi.Domain.Models;
using MarciaApi.Presentation.ViewModel.Items;

namespace MarciaApi.Domain.Repository.Items;

public interface IItemsRepository
{
    Task<List<Item>> Get(int pageNumber);
    Task<Item> Get(string id);
    Task<Item> Generate(ItemsViewModel model);
    Task<List<Item>> GetByName(List<string>? ItemsNames);
}