using SERP.Application.Masters.Lovs.DTOs;
using SERP.Application.Masters.Lovs.DTOs.Request;
using SERP.Application.Masters.Lovs.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.Lovs.Services
{
    public interface ILovService
    {
        Task<IEnumerable<GetByLovTypeResponseDto>> GetByLovType(List<GetByLovTypeRequestDto> lovTypes, bool onlyEnabled);
        Task<PagedResponse<PagedLovValuesResponse>> PagedFilterLovAsync(PagedFilterLovRequestDto request);
        Task CreateLovAsync(string userId, List<CreateLovRequestDto> requests);
        Task UpdateLovAsync(string userId, List<UpdateLovRequestDto> requests);
        Task DeleteLovAsync(List<DeleteLovList> requests);
        Task<IEnumerable<LovDto>> GetAllLoveAsync();
        Task<LovTypeResponseDto> GetAllLovTypeAsync();
    }
}
