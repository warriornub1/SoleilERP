using SERP.Application.Common.Dto;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.Agents.DTOs.Response;
using SERP.Application.Masters.BranchPlants.DTOs;
using SERP.Application.Masters.BranchPlants.DTOs.Request;
using SERP.Application.Masters.BranchPlants.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.BranchPlants.Services
{
    public interface IBranchPlantService
    {
        Task<BranchPlantGetByIdDto> GetById(int id);
        Task<IEnumerable<BranchPlantGetByBUDto>> GetByCompanyAsync(string companyNo);
        Task<List<int>> CreateAsync(string userId, List<CreateBranchPlantRequestDto> request);
        Task UpdateAsync(string userId, List<UpdateBranchPlantRequestDto> request);
        Task DeleteAsync(int id);
        Task DeleteAsync(List<int> ids);
        PagedResponse<PagedBranchPlantResponseDto> PagedFilterBranchPlantAsync(SearchPagedRequestDto requests, PagedFilterBranchPlantRequestDto filters);
    }
}
