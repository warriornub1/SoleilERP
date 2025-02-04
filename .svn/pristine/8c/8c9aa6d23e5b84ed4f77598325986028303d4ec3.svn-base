using Microsoft.EntityFrameworkCore;
using SERP.Application.Finance.RevenueCenters.Interface;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.CostCenters.Model;
using SERP.Domain.Finance.RevenueCenters;
using SERP.Domain.Finance.RevenueCenters.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Finance.RevenueCenters
{
    public class RevenueCenterRepository : GenericRepository<RevenueCenter>, IRevenueCenterRepository
    {
        public RevenueCenterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<RevenueCenterModel>> FindAllCompanyMapping()
        {
            var query = await (from revenueCenter in _dbContext.RevenueCenters.AsNoTracking()
                               join mapping in _dbContext.CompanyStructures.AsNoTracking()
                               on revenueCenter.id equals mapping.company_id into mappingGroup
                               from mapping in mappingGroup.DefaultIfEmpty()
                               select new { revenueCenter })
                             .GroupBy(x => x.revenueCenter)
                             .Select(x => new RevenueCenterModel
                             {
                                 id = x.Key.id,
                                 revenue_center_code = x.Key.revenue_center_code,
                                 revenue_center_description = x.Key.revenue_center_description,
                                 parent_group_id = x.Key.parent_group_id,
                                 status_flag = x.Key.status_flag,
                                 company_structure_id = x.Key.company_structure_id,
                                 created_on = x.Key.created_on,
                                 created_by = x.Key.created_by,
                                 last_modified_on = x.Key.last_modified_on,
                                 last_modified_by = x.Key.last_modified_by,
                             }).ToListAsync();



            return query;
        }

        //public async Task<Tuple<RevenueCenter, IEnumerable<int>>> FindCompanyMapping(int id)
        //{
        //    var query = await (from revenueCenter in _dbContext.RevenueCenters.AsNoTracking()
        //                       join mapping in _dbContext.RevenueCenterCompanyMapping.AsNoTracking()
        //                       on revenueCenter.id equals mapping.revenue_center_id into mappingGroup
        //                       from mapping in mappingGroup.DefaultIfEmpty()
        //                       where revenueCenter.id == id
        //                       select new { revenueCenter, company_id = mapping.company_id })
        //                    .GroupBy(x => x.revenueCenter)
        //                    .Select(x => new
        //                    {
        //                        RevenueCenter = x.Key,
        //                        MappingIds = x.Where(m => m.company_id != null)
        //                                      .Select(m => m.company_id)
        //                                      .ToList()
        //                    })
        //                    .FirstOrDefaultAsync();

        //    if (query == null)
        //        return new Tuple<RevenueCenter, IEnumerable<int>>(null, null);
        //    else
        //        return new Tuple<RevenueCenter, IEnumerable<int>>(query.RevenueCenter, query.MappingIds);
        //}


        public async Task<IEnumerable<RevenueCenterModel>> RevenueCenterFilterPaged(DateTime? createDatefrom, DateTime? createDateTo,
                                                                    List<int> groupList,
                                                                    List<string> statusList, string keyword)
        {
            var queryCC = _dbContext.RevenueCenters.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                queryCC = queryCC.Where(x => x.revenue_center_code.Contains(keyword) || x.revenue_center_description.Contains(keyword));

            if (createDatefrom.HasValue)
                queryCC = queryCC.Where(x => x.created_on >= createDatefrom);

            if (createDateTo.HasValue)
                queryCC = queryCC.Where(x => x.created_on <= createDateTo);

            if (groupList is not null && groupList.Count() > 0)
                queryCC = queryCC.Where(x => groupList.Contains(x.parent_group_id));

            if (statusList is not null && statusList.Count() > 0)
                queryCC = queryCC.Where(x => statusList.Contains(x.status_flag));


            var query = await (from costCenter in queryCC
                               select new { costCenter })
                .GroupBy(x => x.costCenter)
                .Select(x => new RevenueCenterModel
                {
                    id = x.Key.id,
                    revenue_center_code = x.Key.revenue_center_code,
                    revenue_center_description = x.Key.revenue_center_description,
                    parent_group_id = x.Key.parent_group_id,
                    status_flag = x.Key.status_flag,
                    company_structure_id = x.Key.company_structure_id,
                    created_on = x.Key.created_on,
                    created_by = x.Key.created_by,
                    last_modified_on = x.Key.last_modified_on,
                    last_modified_by = x.Key.last_modified_by,
                }).ToListAsync();

            return query;

        }

        public async Task<IEnumerable<string>> GetCode(List<(string, int)> RevenueCodes)
        {
            var query = _dbContext.RevenueCenters.AsQueryable();

            foreach(var revenue in RevenueCodes)
            {
                query = query.Where(x => x.revenue_center_code == revenue.Item1 && x.id != revenue.Item2);
            }

            return await query.Select(x => x.revenue_center_code).ToListAsync();
        }

        public async Task<IEnumerable<RevenueCenter>> GetRevenueCenterViaParentID(int parentId)
        {
            return await _dbContext.RevenueCenters.Where(x => x.parent_group_id == parentId)
                                           .OrderBy(x => x.revenue_center_description)
                                           .ToListAsync();
        }

        public async Task<Dictionary<string, RevenueCenter>> GetDictionary(List<string> rcCodes)
        {
            return await _dbContext.RevenueCenters.Where(x => rcCodes.Contains(x.revenue_center_code))
                .ToDictionaryAsync(x => x.revenue_center_code, x => x);
        }

        public async Task<IEnumerable<CompanyStructureRevenueCenterDb>> FindCompanyStructureRC()
        {
            var result = from cs in _dbContext.CompanyStructures
                         join rc in _dbContext.RevenueCenters
                         on cs.id equals rc.company_structure_id into grouped
                         from rc in grouped.DefaultIfEmpty()
                         select new CompanyStructureRevenueCenterDb
                         {
                             company_structure_id = cs.id,
                             org_no = cs.org_no,
                             org_code = cs.org_code,
                             org_description = cs.org_description,
                             revenue_center_id = rc.id,
                             parent_id = cs.parent_id,
                             org_type = cs.org_type,
                         };

            return await result.OrderBy(x => x.company_structure_id)
                               .ThenBy(x => x.org_no)
                               .ToListAsync();

            //var result = from rc in _dbContext.RevenueCenters
            //             join cs in _dbContext.CompanyStructures
            //             on rc.company_structure_id equals cs.id into grouped
            //             from cs in grouped.DefaultIfEmpty()
            //             select new CompanyStructureRevenueCenterDb
            //             {
            //                 company_structure_id = rc.company_structure_id,
            //                 org_code = cs.org_no,
            //                 org_description = cs.org_description,
            //                 revenue_center_id = rc.id,
            //                 parent_id = cs.parent_id,
            //                 org_type = cs.org_type,
            //                 sequence = cs.sequence,
            //             };

            //return await result.OrderBy(x => x.company_structure_id)
            //             .ThenBy(x => x.org_type)
            //             .ThenBy(x => x.sequence)
            //             .ToListAsync();
        }

        public async Task<Dictionary<int, RevenueCenter>> GetDictionaryViaId(List<int> rcCodes)
        {
            return await _dbContext.RevenueCenters.Where(x => rcCodes.Contains(x.id) && x.status_flag == DomainConstant.StatusFlag.Enabled)
                                                  .ToDictionaryAsync(x => x.id, x => x);
        }

        public async Task<HashSet<int>> GetOrganizationNo(List<int> companyStructureNo)
        {
            var result =  await _dbContext.RevenueCenters.Where(x => companyStructureNo.Contains(x.company_structure_id.Value))
                                                         .Select(x => x.company_structure_id.Value)
                                                         .ToListAsync();

            return new HashSet<int>(result);
        }
    }
}
