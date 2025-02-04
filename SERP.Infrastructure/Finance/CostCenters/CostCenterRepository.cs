using Microsoft.EntityFrameworkCore;
using SERP.Application.Finance.CostCenters.Interface;
using SERP.Domain.Common.Constants;
using SERP.Domain.Finance.CostCenters;
using SERP.Domain.Finance.CostCenters.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Finance.CostCenters
{
    public class CostCenterRepository : GenericRepository<CostCenter>, ICostCenterRepository
    {
        public CostCenterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<CostCenterModel>> FindAllCompanyMapping()
        {

            var query = await (from costCenter in _dbContext.CostCenters.AsNoTracking()
                               join companyStruct in _dbContext.CompanyStructures.AsNoTracking()
                               on costCenter.company_structure_id equals companyStruct.id into mappingGroup
                               //from mapping in mappingGroup.DefaultIfEmpty()
                               select new { costCenter })
                            .GroupBy(x => x.costCenter)
                            .Select(x => new CostCenterModel
                            {
                                id = x.Key.id,
                                cost_center_code = x.Key.cost_center_code,
                                cost_center_description = x.Key.cost_center_description,
                                parent_group_id = x.Key.parent_group_id,
                                status_flag = x.Key.status_flag,
                                created_on = x.Key.created_on,
                                created_by = x.Key.created_by,
                                last_modified_on = x.Key.last_modified_on,
                                last_modified_by = x.Key.last_modified_by,
                            }).ToListAsync();

            return query;

        }

        //public async Task<Tuple<CostCenter, IEnumerable<int>>> FindCompanyMapping(int id)
        //{

        //    var query = await (from costCenter in _dbContext.CostCenters.AsNoTracking()
        //                       join mapping in _dbContext.CostCenterCompanyMapping.AsNoTracking()
        //                       on costCenter.id equals mapping.cost_center_id into mappingGroup
        //                       from mapping in mappingGroup.DefaultIfEmpty()
        //                       where costCenter.id == id
        //                       select new { costCenter, company_id = mapping.company_id })
        //                    .GroupBy(x => x.costCenter)
        //                    .Select(x => new
        //                    {
        //                        CostCenter = x.Key,
        //                        MappingIds = x.Where(m => m.company_id != null)
        //                                      .Select(m => m.company_id)
        //                                      .ToList()
        //                    })
        //                    .FirstOrDefaultAsync();

        //    if (query == null)
        //        return new Tuple<CostCenter, IEnumerable<int>>(null, null);
        //    else
        //        return new Tuple<CostCenter, IEnumerable<int>>(query.CostCenter, query.MappingIds);

        //}

        public async Task<CostCenterModel> FindCompanyMapping(int id)
        {

            var query = await (from costCenter in _dbContext.CostCenters.AsNoTracking()
                    join companyStruct in _dbContext.CompanyStructures.AsNoTracking()
                    on costCenter.company_structure_id equals companyStruct.id into mappingGroup
                    where costCenter.id == id
                    select new { costCenter })
                .GroupBy(x => x.costCenter)
                .Select(x => new CostCenterModel
                {
                    id = x.Key.id,
                    cost_center_code = x.Key.cost_center_code,
                    cost_center_description = x.Key.cost_center_description,
                    parent_group_id = x.Key.parent_group_id,
                    status_flag = x.Key.status_flag,
                    created_on = x.Key.created_on,
                    created_by = x.Key.created_by,
                    last_modified_on = x.Key.last_modified_on,
                    last_modified_by = x.Key.last_modified_by,
                    //company_id = x.Where(m => m.company_id != null)
                    //    .Select(m => new Company_List_CostCenter_Model { company_id = m.company_id })
                    //    .ToList()
                }).FirstOrDefaultAsync();

            return query;

        }

        public async Task<IEnumerable<CostCenterModel>> CostCenterFilterPaged(DateTime? createDatefrom, DateTime? createDateTo,
                                                                            List<int> groupList,
                                                                            List<string> statusList, string keyword)
        {
            var queryCC = _dbContext.CostCenters.AsQueryable();
            var queryCS = _dbContext.CompanyStructures.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                queryCC = queryCC.Where(x => x.cost_center_code.Contains(keyword) || x.cost_center_description.Contains(keyword));

            if (createDatefrom.HasValue)
                queryCC = queryCC.Where(x => x.created_on >= createDatefrom);

            if(createDateTo.HasValue)
                queryCC = queryCC.Where(x => x.created_on <= createDateTo);

            if (groupList is not null && groupList.Count() > 0)
                queryCC = queryCC.Where(x => groupList.Contains(x.parent_group_id));

            if (statusList is not null && statusList.Count() > 0)
                queryCC = queryCC.Where(x => statusList.Contains(x.status_flag));


            var query = await (from costCenter in queryCC
                               select new { costCenter })
                .GroupBy(x => x.costCenter)
                .Select(x => new CostCenterModel
                {
                    id = x.Key.id,
                    cost_center_code = x.Key.cost_center_code,
                    cost_center_description = x.Key.cost_center_description,
                    parent_group_id = x.Key.parent_group_id,
                    status_flag = x.Key.status_flag,
                    created_on = x.Key.created_on,
                    created_by = x.Key.created_by,
                    last_modified_on = x.Key.last_modified_on,
                    last_modified_by = x.Key.last_modified_by,
                }).ToListAsync();

            return query;

        }

        public async Task<IEnumerable<string>> GetCode(List<(string, int)> RevenueCodes)
        {
            var query = _dbContext.CostCenters.AsQueryable();

            foreach (var revenue in RevenueCodes)
            {
                query = query.Where(x => x.cost_center_code == revenue.Item1 && x.id != revenue.Item2);
            }

            return await query.Select(x => x.cost_center_code).ToListAsync();
        }

        public async Task<IEnumerable<CostCenter>> GetCostCenterViaParentID(int parentId)
        {
            return await _dbContext.CostCenters.Where(x => x.parent_group_id == parentId)
                                           .OrderBy(x => x.cost_center_description)
                                           .ToListAsync();
        }

        public async Task<Dictionary<string, CostCenter>> GetDictionary(List<string> ccCodes)
        {
            return await _dbContext.CostCenters.Where(x => ccCodes.Contains(x.cost_center_code))
                .ToDictionaryAsync(x => x.cost_center_code, x => x);
        }

        public async Task<IEnumerable<CompanyStructureCostCenterDb>> FindCompanyStructureCC()
        {
            var result = from cs in _dbContext.CompanyStructures
                         join cc in _dbContext.CostCenters
                         on cs.id equals cc.company_structure_id into grouped
                         from cc in grouped.DefaultIfEmpty()
                         select new CompanyStructureCostCenterDb
                         {
                             company_structure_id = cs.id,
                             org_no = cs.org_no,
                             org_code = cs.org_code,
                             org_description = cs.org_description,
                             cost_center_id = cc.id,
                             parent_id = cs.parent_id,
                             org_type = cs.org_type,
                         };

            return await result.OrderBy(x => x.company_structure_id)
                               .ThenBy(x => x.org_no)
                               .ToListAsync();

        }

        public async Task<Dictionary<int, CostCenter>> GetDictionaryViaId(List<int> rcCodes)
        {
            return await _dbContext.CostCenters.Where(x => rcCodes.Contains(x.id) && x.status_flag == DomainConstant.StatusFlag.Enabled)
                                                  .ToDictionaryAsync(x => x.id, x => x);
        }
    }
}
