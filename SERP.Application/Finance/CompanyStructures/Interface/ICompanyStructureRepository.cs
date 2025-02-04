using SERP.Application.Common;
using SERP.Application.Finance.CompanyStructures.DTOs.Response;
using SERP.Domain.Finance.CompanyStructures;

namespace SERP.Application.Finance.CompanyStructures.Interface
{
    public interface ICompanyStructureRepository : IGenericRepository<CompanyStructure>
    {
        Task<IEnumerable<int>> GetCompanyStructureCodes(List<int> companyStructureId);
        Task<Dictionary<string, int>> GetCompanyOrgNoDic(List<string> orgNumber);
        Task<Dictionary<int, string>> GetCompanyOrgIdDic(List<int> idNos);
        Task<IEnumerable<SearchCompanyStructureResponseModel>> GetCompanyStructSearchPage(string keyword, string sortBy, bool sortAscending,
                                                        List<int> companyList, List<int> orgList, List<string> statusList,
                                                        List<int> inChargeEmployeeIdList, List<int> departmentList, List<int> divisonList, List<int> sectionList);

        Task<IEnumerable<CompanyStructure>> GeCompanyStructureLimited(int company_id, int? org_type, bool onlyEnabled);
    }
}
