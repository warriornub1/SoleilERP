using SERP.Application.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Application.Transactions.Receiving.DTOs.Request;
using SERP.Application.Transactions.Receiving.DTOs.Response;
using SERP.Domain.Common.Model;
using SERP.Domain.Transactions.Receiving;

namespace SERP.Application.Transactions.Receiving.Services
{
    public interface IReceivingService
    {
        Task<int> CreateReceivingAsync(string userId, CreateReceivingRequestDto request);
        Task UpdateReceivingAsync(string userId, UpdateReceivingRequestDto request);
        Task DeleteReceivingLineAsync(string userId, int receivingLineId);
        Task UpdateByActionAsync(string userId, UpdateReceivingByActionDto request);
        Task DeleteReceivingAsync(string userId, int receivingHeaderId);
        Task UploadFileAsync(string userId, int receivingHeaderId, int receivingDetailId, List<FileInfoRequestDto> fileList);
        Task<List<string>> DeleteFilesAsync(string userId, DeleteReceivingFileDto request);
        Task<PagedResponse<PagedReceivingResponseDto>> PagedFilterReceivingAsync(SearchPagedRequestDto requests, PagedFilterReceivingRequestDto filters);
        Task<List<DropdownListResponseDto>> GetReceivingListAsync(GetReceivingListRequestDto request);
        Task<DocumentListResponseDto> GetDocumentList(int rcvHeaderId);
        Task<ReceivingGetByIdFileUrlResponseDTO> GetByReceivingHeaderIdAsync(int rcvHeaderId);
        Task<ReceivingGetByIdFileUrlResponseDTO> GetByReceivingNoAsync(string receivingNo);
        Task<ReceivingItemListResponseDto> GetItemListAsync(int rcvHeaderId, string? documentNo, string? packageNo);
        Task<ReceivingLineNoResponseDto> GetReceivingDetailLineByItemAsync(int rcvHeaderId, int itemId);
        Task<ReceivingItemDetailResponseDto> GetItemDetailsAsync(int rcvDetailId);
        Task<ReceivingDetailFileUrlResponseDto> SearchDetailAsync(int rcvHeaderId, string keyword, GetReceivingListRequestDto request);

        Task CreateReceivingDetail(string userId, IUnitOfWork unitOfWork, List<ReceivingDetail> requests);
        Task<int> CreateReceivingHeader(string userId, IUnitOfWork unitOfWork, ReceivingHeader request);
    }
}