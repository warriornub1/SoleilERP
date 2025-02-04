using SERP.Application.Common;
using SERP.Domain.Masters.Companies;
using System.Linq.Expressions;

namespace SERP.Application.Masters.Companies.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<IEnumerable<Company>> CompanyFilterPaged(DateTime? createDateFrom, DateTime? createDateTo,
                                                                    List<int> parentList, List<int> currencyList,
                                                                    List<bool> flagList, List<bool> dormantFlagList,
                                                                    List<string> statusList, string keyword);
        Task<List<Company>> GetAllLimitedAsync(bool onlyEnabled);
        Task<int[]> GetCompanyAvailableAsync(HashSet<int> companyIds);
        Task<IEnumerable<Company>> GetCompanyViaParentID(int parentId);
        Task<IEnumerable<string>> GetDuplicatedCompanyNames(List<(int, string)> companyValues);
        Task<(List<int>, List<Company>)> CheckForMissingId(List<int> ids);
        Task<IEnumerable<Company>> GetCompanyListDB();
        Task<IDictionary<string, Company>> GetCompanyNoDictionary(List<string?> companyNo);
        Task<IDictionary<int, Company>> GetCompanyIdDictionary(List<int> companyId);
        Task<IEnumerable<int>> GetCompanyIdList(List<int> companyId);
    }
}
