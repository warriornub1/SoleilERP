using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.PackingLists.Interfaces;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Transactions.PackingLists;
using SERP.Domain.Transactions.PackingLists.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.PackingLists
{
    internal class ASNPackingDiscrepancyRepository : GenericRepository<ASNPackingDiscrepancy>, IASNPackingDiscrepancyRepository
    {
        public ASNPackingDiscrepancyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<PackingDiscrepancyListResponseDetail> BuildPackingDiscrepancyListQuery(FilterPackingListRequestModel request)
        {
            var asnPackingListQuery = _dbContext.ASNPackingDiscrepancy.AsNoTracking()
                .Where(x => x.asn_header_id == request.AsnHeaderId);

            var query = from asnPackingList in asnPackingListQuery
                        join item in _dbContext.Item.AsNoTracking() on asnPackingList.item_id equals item.id
                        join itemMapping in _dbContext.SupplierItemMapping.AsNoTracking() on item.id equals itemMapping.item_id
                        join countryOfOrigin in _dbContext.Country.AsNoTracking() on asnPackingList.country_of_origin_id equals
                            countryOfOrigin.id into lstCountryOfOrigin
                        from countryOfOrigin in lstCountryOfOrigin.DefaultIfEmpty()
                        select new PackingDiscrepancyListResponseDetail
                        {
                            id = asnPackingList.id,
                            item_no = item.item_no,
                            description_1 = item.description_1,
                            supplier_part_no = itemMapping.supplier_part_no,
                            asn_qty = asnPackingList.asn_qty,
                            packing_list_qty = asnPackingList.packing_list_qty,
                            uom = item.primary_uom,
                            country_of_origin = countryOfOrigin != null
                                ? new CountryBasicDetail
                                {
                                    id = countryOfOrigin.id,
                                    country_name = countryOfOrigin.country_name,
                                    country_alpha_code_two = countryOfOrigin.country_alpha_code_two,
                                    country_alpha_code_three = countryOfOrigin.country_alpha_code_three
                                }
                                : null
                        };

            // - Keywords are Item No, Description 1, Supplier Part No
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                    x.item_no.Contains(request.Keyword) ||
                    x.description_1.Contains(request.Keyword) ||
                    x.supplier_part_no.Contains(request.Keyword));
            }

            return query;
        }

        public async Task<List<ASNPackingDiscrepancy>> GetAsnPackingDiscrepancytByAsnHeaderIdsAsync(HashSet<int> asnHeaderIds)
        {
            if (asnHeaderIds.Count == 0)
            {
                return [];
            }

            return await _dbContext.ASNPackingDiscrepancy.Where(x => asnHeaderIds.Contains(x.asn_header_id)).ToListAsync();
        }
    }
}
