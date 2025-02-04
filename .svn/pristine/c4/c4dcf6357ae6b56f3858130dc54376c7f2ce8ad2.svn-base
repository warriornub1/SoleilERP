using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Domain.Masters.Items;
using SERP.Domain.Masters.Items.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Items
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Item>> GetAllLimited(bool onlyEnabled)
        {
            return await _dbContext.Item.Where(x => !onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))
                .OrderBy(x => x.item_no).ToListAsync();
        }

        public async Task<ItemDetail?> GetById(int id)
        {
            var query = from item in _dbContext.Item.AsNoTracking()
                        join itemUom in _dbContext.ItemUomConversion.AsNoTracking() on item.id equals itemUom.item_id into itemUomGroup
                        from itemUom in itemUomGroup.DefaultIfEmpty()
                        where item.id == id
                        select new ItemDetail
                        {
                            id = item.id,
                            item_no = item.item_no,
                            description_1 = item.description_1,
                            description_2 = item.description_2,
                            brand = item.brand,
                            primary_uom = item.primary_uom,
                            secondary_uom = item.secondary_uom,
                            purchasing_uom = item.purchasing_uom,
                            purchase_min_order_qty = item.purchase_min_order_qty,
                            purchase_multiple_order_qty = item.purchase_multiple_order_qty,
                            item_conversion = new ItemConversionDetail
                            {
                                primary_uom_qty = itemUom != null ? itemUom.primary_uom_qty : 0,
                                secondary_uom_qty = itemUom != null ? itemUom.secondary_uom_qty : 0
                            },
                            label_required_flag = item.label_required_flag,
                            lot_control_flag = item.lot_control_flag,
                            inspection_instruction = item.inspection_instruction,
                            status_flag = item.status_flag,
                            created_on = item.created_on,
                            created_by = item.created_by,
                            last_modified_on = item.last_modified_on,
                            last_modified_by = item.last_modified_by
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int[]> GetItemAvailable(HashSet<int> itemIDs)
        {
            return await _dbContext.Item
                .Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled
                            && itemIDs.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }

        public async Task<Dictionary<string, Item>> GetDictionaryByItemNo(List<string?> itemNos)
        {
           return await _dbContext.Item
                .Where(x => itemNos.Contains(x.item_no))
                .ToDictionaryAsync(x => x.item_no);
        }

        public async Task<List<Item>> GetItemListAsync(HashSet<string> itemNos)
        {
            return await  _dbContext.Item
                .Where(x => itemNos.Contains(x.item_no))
                .Select(x => new Item
                {
                    id = x.id,
                    item_no = x.item_no
                }).ToListAsync();
        }

        public async Task<List<Item>> GetItemListByIdsAsync(List<int> itemIds)
        {
            return await _dbContext.Item
                .Where(x => itemIds.Contains(x.id))
                .ToListAsync();
        }
    }
}
