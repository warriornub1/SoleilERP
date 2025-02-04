using SERP.Application.Common;
using SERP.Domain.Finance.RevenueCenters;
using SERP.Domain.Finance.RevenueCenters.Model;

namespace SERP.Application.Finance.RevenueCenters.Interface
{
    public interface IRevenueCenterRepository : IGenericRepository<RevenueCenter>
    {
        Task<IEnumerable<RevenueCenterModel>> FindAllCompanyMapping();
        Task<IEnumerable<RevenueCenterModel>> RevenueCenterFilterPaged(DateTime? createDatefrom, DateTime? createDateTo,
                                                                            List<int> groupList,
                                                                            List<string> statusList, string keyword);
        Task<IEnumerable<string>> GetCode(List<(string, int)> RevenueCodes);

        Task<IEnumerable<RevenueCenter>> GetRevenueCenterViaParentID(int parentId);

        Task<Dictionary<string, RevenueCenter>> GetDictionary(List<string> rcCodes);
        Task<IEnumerable<CompanyStructureRevenueCenterDb>> FindCompanyStructureRC();
        Task<Dictionary<int, RevenueCenter>> GetDictionaryViaId(List<int> rcCodes);
        Task<HashSet<int>> GetOrganizationNo(List<int> companyStructureNo);
    }
}
