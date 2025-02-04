using Microsoft.EntityFrameworkCore;
using SERP.Application.Finance.Natural_Accounts.Interface;
using SERP.Domain.Finance.NaturalAccounts;
using SERP.Domain.Finance.NaturalAccounts.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Reflection.Metadata.Ecma335;

namespace SERP.Infrastructure.Finance.Natural_Accounts
{
    public class NaturalAccountRepository : GenericRepository<NaturalAccount>, INaturalAccountRepository
    {
        public NaturalAccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<NaturalAccountModel>> FindAllNaturalAccount()
        {
            var query = await _dbContext.NaturalAccounts.AsNoTracking().Select(x =>
                                new NaturalAccountModel
                                {
                                    id = x.id,
                                    natural_account_code = x.natural_account_code,
                                    natural_account_description = x.natural_account_description,
                                    natural_account_type = x.natural_account_type,
                                    parent_group_id = x.parent_group_id,
                                    status_flag = x.status_flag,
                                    created_on = x.created_on,
                                    created_by = x.created_by,
                                    last_modified_on = x.last_modified_on,
                                    last_modified_by = x.last_modified_by,
                                }).ToListAsync();
            return query;
        }

        public async Task<IEnumerable<NaturalAccountModel>> NaturalAccountFilterPaged(DateTime? createDatefrom, DateTime? createDateTo,
                                                            List<int> groupList, List<string> typeList,
                                                            List<string> statusList, string keyword)
        {
            var queryNA = _dbContext.NaturalAccounts.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                queryNA = queryNA.Where(x => x.natural_account_code.Contains(keyword) || x.natural_account_description.Contains(keyword));


            if (createDatefrom.HasValue)
                queryNA = queryNA.Where(x => x.created_on >= createDatefrom);

            if (createDateTo.HasValue)
                queryNA = queryNA.Where(x => x.created_on <= createDateTo);

            if (groupList is not null && groupList.Count() > 0)
                queryNA = queryNA.Where(x => groupList.Contains(x.parent_group_id));

            if (typeList is not null && typeList.Count() > 0)
                queryNA = queryNA.Where(x => typeList.Contains(x.natural_account_type));

            if (statusList is not null && statusList.Count() > 0)
                queryNA = queryNA.Where(x => statusList.Contains(x.status_flag));


            var query = await queryNA.Select(x => new NaturalAccountModel
            {
                id = x.id,
                natural_account_code = x.natural_account_code,
                natural_account_description = x.natural_account_description,
                natural_account_type = x.natural_account_type,
                parent_group_id = x.parent_group_id,
                status_flag = x.status_flag,
                created_on = x.created_on,
                created_by = x.created_by,
                last_modified_on = x.last_modified_on,
                last_modified_by = x.last_modified_by,
            }).ToListAsync();

            return query;

        }

        public async Task<IEnumerable<string>> NaturalAccountGetCodes(List<(string, int)> inputFilters)
        {

            var matches = from filter in inputFilters
                          join account in _dbContext.NaturalAccounts.AsNoTracking()
                          on filter.Item1 equals account.natural_account_code
                          where filter.Item2 != account.id
                          select account.natural_account_code;



            return matches.ToList();
        }

        public async Task<IEnumerable<NaturalAccount>> GetNaturalAccountViaParentID(int parentId)
        {
            return await _dbContext.NaturalAccounts.Where(x => x.parent_group_id == parentId)
                                           .OrderBy(x => x.natural_account_description)
                                           .ToListAsync();
        }

        public async Task<Dictionary<string, NaturalAccount>> GetNaturalAccountDictionary(List<string> codes)
        {
            return await _dbContext.NaturalAccounts.Where(x => codes.Contains(x.natural_account_code))
                                                   .ToDictionaryAsync(x => x.natural_account_code, x => x);
        }

    }
}
