using SERP.Application.Finance.Natural_Accounts.DTOs.Request;
using SERP.Application.Finance.Natural_Accounts.DTOs.Response;
using SERP.Domain.Common.Model;
using SERP.Domain.Finance.NaturalAccounts.Model;

namespace SERP.Application.Finance.Natural_Accounts.Services
{
    public interface INaturalAccountService
    {
        Task<PagedResponse<NaturalAccountModel>> GetAllPagedAsync(int page, int pageSize);
        Task<PagedResponse<NaturalAccountResponseModel>> SearchPagedAsync(int page, int pageSize, string keyword, SearchNaturalAccountRequestModel request);
        Task<NaturalAccountResponseModel> GetByIdAsync(int id);
        Task CreateNaturalAccountAsync(CreateNaturalAccountRequestModel request, string userId);
        Task UpdateNaturalAccountAsync(List<UpdateNaturalAccountRequestModel> requests, string userId);
        Task<byte[]> GetNaturalAccountTemplate(string webRootPath);
        Task DeleteNaturalAccountAsync(DeleteNaturalAccountRequestModel request);
        Task ImportNaturalAccountAsync(string userId, ImportNaturalAccountRequestModel request);
        Task<IEnumerable<NaturalAccountTreeResponseModel>> GetNaturalAccountTreeViewAsync();
    }
}
