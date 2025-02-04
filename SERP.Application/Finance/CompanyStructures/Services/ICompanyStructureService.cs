using SERP.Application.Finance.CompanyStructures.DTOs.Request;
using SERP.Application.Finance.CompanyStructures.DTOs.Response;
using SERP.Application.Finance.CostCenters.DTOs.Request;
using SERP.Application.Finance.Employees.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Finance.CompanyStructures.Services
{
    public interface ICompanyStructureService
    {
        Task<PagedResponse<SearchCompanyStructureResponseModel>> SearchPagedAsync(int page, int pageSize, string keyword, string sortBy, bool sortAscending, SearchCompanyStructureRequestModel request);
        Task<IEnumerable<CompanyStructureResponseModel>> GetAllLimitedAsync(int company_id, int? org_type, bool onlyEnabled);
        Task DeleteAsync(DeleteCompanyStructureRequest request);
    }
}
