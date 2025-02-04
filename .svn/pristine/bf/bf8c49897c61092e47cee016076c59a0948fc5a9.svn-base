using SERP.Application.Masters.Companies.DTOs.Request;
using SERP.Application.Masters.Companies.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.Companies.Services
{
    public interface ICompanyService
    {
        Task<CompanyDetailResponseDto> GetByIdAsync(int id);
        Task<List<CompanyResponseDto>> GetAllLimitedAsync(bool onlyEnabled);
        Task<PagedResponse<CompanyResponseModel>> GetAllPagedAsync(int page, int pageSize);
        Task<PagedResponse<CompanyPagedResponseDto>> SearchPagedAsync(int page, int pageSize, string keyword, SearchCompanyPagedRequestModel request);
        Task<IEnumerable<CompanyTreeResponseDto>> GetCompanyTreeViewAsync();
        Task CreateCompanyAsync(CompanyCreateRequestDto request, string userId);
        Task UpdateCompanyAsync(List<UpdateCompanyRequestDto> requests, string userId);
        Task DeleteCompanyAsync(DeleteCompanyRequestsDto requests);
        Task<CompanyListResponseDto> GetCompanyListAsync();
        Task ImportCompanyGroupAsync(string userId, ImportCompanyRequestDto request);
        Task<byte[]> GetCompanyTemplateAsync(string webRootPath);
    }
}
