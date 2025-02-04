using SERP.Application.Common;
using SERP.Domain.Finance.NaturalAccounts;
using SERP.Domain.Finance.NaturalAccounts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Application.Finance.Natural_Accounts.Interface
{
    public interface INaturalAccountRepository : IGenericRepository<NaturalAccount>
    {
        Task<IEnumerable<NaturalAccountModel>> FindAllNaturalAccount();
        Task<IEnumerable<NaturalAccountModel>> NaturalAccountFilterPaged(DateTime? createDatefrom, DateTime? createDateTo,
                                                                    List<int> groupList, List<string> typeList,
                                                                    List<string> statusList, string keyword);

        Task<IEnumerable<string>> NaturalAccountGetCodes(List<(string, int)> inputFilters);


        Task<IEnumerable<NaturalAccount>> GetNaturalAccountViaParentID(int parentId);

        Task<Dictionary<string, NaturalAccount>> GetNaturalAccountDictionary(List<string> codes);
    }
}
