using SERP.Application.Common;
using SERP.Domain.Masters.Items;

namespace SERP.Application.Masters.Items.Interfaces
{
    public interface IItemUomConversionRepository : IGenericRepository<ItemUomConversion>
    {
        Task<List<ItemUomConversion>> GetItemUomConversionListAsync(List<int> itemIds);
    }
}
