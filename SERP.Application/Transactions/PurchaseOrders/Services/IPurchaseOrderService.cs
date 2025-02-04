using SERP.Application.Common;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.PurchaseOrders.Services
{
    public interface IPurchaseOrderService
    {
        Task<PagedResponse<PagedPoResponseDto>> PagedFilterPoAsync(PagedFilterPoRequestDto request);
        Task<PoDetailResponseDto> GetByIdAsync(int id);
        Task<PoDetailResponseDto> GetByPoNoAsync(string poNo);
        Task<int[]> CreatePoAsync(string userId, CreatePoRequestDto request);
        Task UpdatePoAsync(string userId, UpdatePoRequestDto request);
        Task<int[]> UploadFileAsync(string userId, UploadFileRequestDto request);
        Task<List<string>> RemoveFileAsync(int poHeaderId, List<int> poFileIDs);
        Task DeletePOLineAsync(string userId, DeletePoLineRequestDto request);
        Task<PagedResponse<PagePoDetailResponseDto>> SearchDetailPaged(PagedFilterPoRequestDto request);
        Task<List<PoNoListResponseDto>> GetPoNoListAsync(PoNoRequestDto request);
        Task DeletePOAsync(string userId, int poHeaderId);
        Task UpdatePoHeaderStatusToNewByPODetailStatus(string userId, IUnitOfWork unitOfWork, int poHeaderId);
        Task ClosePoAsync(string userId, int poHeaderId);
        Task ClosePoLineAsync(string userId, DeletePoLineRequestDto request);
    }
}