using SERP.Application.Finance.Groups.DTOs.Request;
using SERP.Application.Finance.Groups.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Finance.Groups.Services
{
    public interface IGroupService
    {
        Task<GroupResponseModel> GetByIdAsync(int id);
        Task CreateGroupAsync(string userId, CreateGroupRequestModel request);
        Task UpdateGroupAsync(string userId, List<UpdateGroupRequestModel> requests);
        Task DeleteGroupAsync(DeleteGroupRequestModel request);
        Task<PagedResponse<GroupResponseModel>> GetAllPagedAsync(int page, int pageSize, string groupType);
        Task<PagedResponse<GroupResponseModel>> SearchPagedAsync(int page, int pageSize, string groupType, string keyword, SearchPagedGroupRequestModel request);
        Task ImportCompanyGroupAsync(string userId, ImportCompanyGroupModel request);
        Task ImportCostCenterGroupAsync(string userId, ImportCompanyGroupModel request);
        Task ImportRevenueCenterGroupAsync(string userId, ImportCompanyGroupModel request);
        Task ImportNaturalAccountGroupAsync(string userId, ImportCompanyGroupModel request);
        Task<GroupFilterByGroupType> GetGroupListByGroupTypeAsync(string groupType);
        Task<GroupTypeParentGroupModel> GetParentGroupListByGroupTypeAsync(string groupType);
        Task<(byte[], string)> GetGroupTemplate();
    }
}
