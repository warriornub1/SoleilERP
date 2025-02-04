using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Suppliers
{
    internal class SupplierItemMappingRepository : GenericRepository<SupplierItemMapping>, ISupplierItemMappingRepository
    {
        public SupplierItemMappingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Dictionary<Tuple<int, int, string?>, SupplierItemMapping>> GetDictionarySupplierItemMappingAsync(HashSet<int> supplierIds, HashSet<int> itemIds, HashSet<string?> supplierMaterialCodes)
        {
            return await _dbContext.SupplierItemMapping
                 .Where(x => supplierIds.Contains(x.supplier_id)
                             || itemIds.Contains(x.item_id)
                             || supplierMaterialCodes.Contains(x.supplier_material_code))
                 .ToDictionaryAsync(x => new Tuple<int, int, string?>(x.supplier_id, x.item_id, x.supplier_material_code));
        }

        public async Task<int[]> GetSupplierItemMappingAvailable(HashSet<int> supplierMappingIds)
        {
            return await _dbContext.SupplierItemMapping
                .Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled &&
                            supplierMappingIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }

        public IQueryable<SupplierItemMappingPagedResponseDetail> BuildSupplierItemMappingFilterQuery(FilterModel request)
        {
            var itemMappingQuery = _dbContext.SupplierItemMapping.AsNoTracking()
                .Where(x => x.supplier_id == request.supplier_id);

            if (request.default_flag.HasValue)
            {
                itemMappingQuery = itemMappingQuery.Where(x => x.default_flag == request.default_flag.Value);
            }

            if (request.create_date_from.HasValue)
            {
                itemMappingQuery = itemMappingQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                itemMappingQuery = itemMappingQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                itemMappingQuery = itemMappingQuery.Where(x => request.status_flag.Contains(x.status_flag));
            }

            var query = from itemMapping in itemMappingQuery
                        join item in _dbContext.Item.AsNoTracking() on itemMapping.item_id equals item.id into itemGroup
                        from item in itemGroup.DefaultIfEmpty()
                        select new SupplierItemMappingPagedResponseDetail
                        {
                            supplier_item_mapping_id = itemMapping.id,
                            item_no = item.item_no,
                            description_1 = item.description_1,
                            description_2 = item.description_2,
                            supplier_part_no = itemMapping.supplier_part_no,
                            supplier_material_code = itemMapping.supplier_material_code,
                            supplier_material_description = itemMapping.supplier_material_description,
                            default_flag = itemMapping.default_flag,
                            status_flag = itemMapping.status_flag,
                            created_on = itemMapping.created_on,
                            created_by = itemMapping.created_by,
                            last_modified_on = itemMapping.last_modified_on,
                            last_modified_by = itemMapping.last_modified_by
                        };

            // - Keywords are Item No, Supplier Part No and Description 1 (From Item)
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                    x.item_no.Contains(request.Keyword) ||
                    x.supplier_part_no.Contains(request.Keyword) ||
                    x.description_1.Contains(request.Keyword));
            }

            return query;
        }
    }
}
