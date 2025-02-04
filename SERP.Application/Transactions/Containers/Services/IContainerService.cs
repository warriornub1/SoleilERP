using SERP.Application.Common.Dto;
using SERP.Application.Transactions.Containers.DTOs.Request;
using SERP.Application.Transactions.Containers.DTOs.Response;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.Containers.Services
{
    public interface IContainerService
    {
        Task<int[]> CreateContainerAsync(string userId, List<CreateContainerRequestDto> requests);
        Task DeleteContainerAsync(string userId, int containerIds);
        Task UpdateContainerByActionAsync(string userId, int bpId, string localPath, UpdateContainerByActionRequestDto request);
        Task UpdateContainerAsync(string userId, List<UpdateContainerRequestDto> requests);
        Task<PagedResponse<PagedContainerResponseDto>> PagedFilterContainerAsync(SearchPagedRequestDto requests, PagedFilterContainerRequestDto filters);
        Task<List<DropdownListResponseDto>> GetContainerListAsync(string bpNo, GetContainerListRequestDto request);
        Task<ContainerDetailResponseDto?> GetByIdAsync(int id);
        //Task UploadFileAsync(string userId, int container_id, List<ContainerFileInfoRequestDto> request);
        //Task<List<string>> RemoveFileAsync(RemoveContainerFileDto request);
    }
}
