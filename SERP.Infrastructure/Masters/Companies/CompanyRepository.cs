using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Companies.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.Companies;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Companies
{
    internal class CompanyRepository: GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<Company>> GetAllLimitedAsync(bool onlyEnabled)
        {
            var query = _dbContext.Company.AsNoTracking();

            if (onlyEnabled)
            {
                query = query.Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled);
            }

            return await query.ToListAsync();
        }

        public async Task<int[]> GetCompanyAvailableAsync(HashSet<int> companyIds)
        {
            return await _dbContext.Company
                .Where(x => companyIds.Contains(x.id) && x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))
                .Select(x => x.id).ToArrayAsync();
        }

        public async Task<IEnumerable<Company>> CompanyFilterPaged(DateTime? createDateFrom, DateTime? createDateTo, 
                                                                    List<int> parentList, List<int> currencyList,
                                                                    List<bool> flagList, List<bool> dormantFlagList,
                                                                    List<string> statusList, string keyword)
        {
            var query = _dbContext.Company.AsQueryable();
            
            if(!String.IsNullOrEmpty(keyword) )
            {
                query = query.Where(x => x.company_no.Contains(keyword) || x.company_name.Contains(keyword));
            }

            if(createDateFrom.HasValue)
            {
                query = query.Where( x => x.created_on >= createDateFrom );
            }

            if(createDateTo.HasValue)
            {
                query = query.Where( x => x.created_on <= createDateTo );
            }

            if(parentList is not null && parentList.Count() > 0)
            {
                query = query.Where(x => parentList.Contains(x.parent_group_id));
            }

            if(currencyList is not null && currencyList.Count() > 0)
            {
                query = query.Where(x => currencyList.Contains(x.base_currency_id));
            }
            
            if(flagList is not null && flagList.Count() > 0)
            {
                query = query.Where(x => flagList.Contains(x.intercompany_flag));
            }

            if(dormantFlagList is not null && dormantFlagList.Count() > 0)
            {
                query = query.Where(x => dormantFlagList.Contains(x.dormant_flag));
            }

            if(statusList is not null &&  statusList.Count() > 0)
            {
                query = query.Where(x => statusList.Contains(x.status_flag));
            }

            return await query.OrderBy(x => x.company_no)
                              .ToListAsync();

        }

        public async Task<IEnumerable<Company>> GetCompanyViaParentID(int parentId)
        {
            return await _dbContext.Company.Where(x => x.parent_group_id == parentId)
                                           .OrderBy(x => x.company_no)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetDuplicatedCompanyNames(List<(int, string)> companyValues)
        {
            return companyValues.Where(request =>
                _dbContext.Company.Any(company => company.id != request.Item1 && company.company_no == request.Item2))
                .Select(request => request.Item2)
                .Distinct()
                .ToList();
        }

        public async Task<(List<int>, List<Company>)> CheckForMissingId(List<int> ids)
        {
            var companies = await _dbContext.Company.Where(x => ids.Contains(x.id))
                                           .ToListAsync();


            return (ids.Except(companies.Select(x => x.id)).ToList(), companies);
        }

        public async Task<IEnumerable<Company>> GetCompanyListDB()
        {
            return await _dbContext.Company.Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled)
                                           .OrderBy(x => x.company_no)
                                           .ToListAsync();
        }

        public async Task<IDictionary<string, Company>> GetCompanyNoDictionary(List<string?> companyNo)
        {
            return await _dbContext.Company.Where(x => companyNo.Contains(x.company_no))
                                           .ToDictionaryAsync(x => x.company_no, x => x);
        }

        public async Task<IDictionary<int, Company>> GetCompanyIdDictionary(List<int> companyId)
        {
            return await _dbContext.Company.Where(x => companyId.Contains(x.id))
                                           .ToDictionaryAsync(x => x.id, x => x);
        }

        public async Task<IEnumerable<int>> GetCompanyIdList(List<int> companyId)
        {
            return await _dbContext.Company.Where(x => companyId.Contains(x.id) && x.status_flag == DomainConstant.StatusFlag.Enabled)
                                           .Select(x => x.id)
                                           .ToListAsync();
        }
    }
}
