using SERP.Application.Common.Dto;
using SERP.Application.Transactions.CustomViews.DTOs.Request;
using SERP.Application.Transactions.CustomViews.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.CustomViews.Services
{
    public interface ICustomViewService
    {
        Task<IEnumerable<CustomViewResponseDto>> GetByCustomViewType(string customViewType, string? userId, bool privateFlag = false);
        Task<CustomViewAttributeResponseDto?> GetAttributesByCustomViewId(int customViewId);
        Task<int> CreateCustomView(string userId, CreateCustomViewRequestDto request);
        Task UpdateCustomViewAttributes(string userId, UpdateCustomViewAttributeRequestDto request);
        Task DeleteCustomView(List<int > customViewIds);
        Task UpdateCustomView(string userId, List<UpdateCustomViewRequestDto> request);
        PagedResponse<CustomViewPagedResponseDto> SearchPaged(SearchPagedRequestDto request, FilterCustomViewPagedRequestDto filter);
    }
}
