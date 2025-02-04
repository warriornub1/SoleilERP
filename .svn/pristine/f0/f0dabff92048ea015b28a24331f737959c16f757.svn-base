using SERP.Application.Common;
using SERP.Domain.Masters.Items;
using SERP.Domain.Masters.Items.Model;

namespace SERP.Application.Masters.Items.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllLimited(bool onlyEnabled);
        Task<ItemDetail?> GetById(int id);
        Task<int[]> GetItemAvailable(HashSet<int> itemIDs);
        Task<Dictionary<string, Item>> GetDictionaryByItemNo(List<string?> toList);
        Task<List<Item>> GetItemListAsync(HashSet<string> itemNos);
        Task<List<Item>> GetItemListByIdsAsync(List<int> itemIds);
    }
}
