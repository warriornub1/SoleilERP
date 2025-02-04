using SERP.Application.Common;
using SERP.Domain.Masters.Agents.Model;
using SERP.Domain.Masters.BranchPlants;
using SERP.Domain.Masters.BranchPlants.Models;

namespace SERP.Application.Masters.BranchPlants.Interfaces
{
    public interface IBranchPlantRepository : IGenericRepository<BranchPlant>
    {
        Task<BranchPlantDetail> GetById(int id);
        Task<IEnumerable<BranchPlantDetail>> GetByCompany(string buNo);
        Task<int[]> CheckInvalidBranchPlantIds(HashSet<int> branchPlantIds);
        Task<int[]> GetBranchPlantAvailable(HashSet<int> union);
        Task<bool> CheckExistedBranchPlantNo(string requestBranchPlantNo);
        Task<IEnumerable<int>> FindMapping(List<int> ids);
        Task<int?> GetCompanyIdsByBranchPlantId(int branchPlantId);
        Task<List<BranchPlant>> GetListBranchPlantAsync(List<int> branchPlantIds);
        IQueryable<PagedBranchPlantDetail> BuildFilterBranchPlantQuery(PagedFilterBranchPlantRequestModel request);
		Task<IEnumerable<int>> GetBranchPlantId(List<int> ids);    }
}
