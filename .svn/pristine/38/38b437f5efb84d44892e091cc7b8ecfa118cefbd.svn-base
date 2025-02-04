using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Domain.Masters.Items;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Items
{
    public class ItemUomConversionRepository : GenericRepository<ItemUomConversion>, IItemUomConversionRepository
    {
        public ItemUomConversionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<ItemUomConversion>> GetItemUomConversionListAsync(List<int> itemIds)
        {
            return await _dbContext.ItemUomConversion
                .Where(x => itemIds.Contains(x.item_id))
                .ToListAsync();
        }
    }
}
