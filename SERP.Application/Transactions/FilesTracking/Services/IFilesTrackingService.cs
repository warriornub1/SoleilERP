using Microsoft.AspNetCore.Http;
using SERP.Application.Common;
using SERP.Application.Transactions.FilesTracking.DTOs;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Transactions.FilesTracking;

namespace SERP.Application.Transactions.FilesTracking.Services
{
    public interface IFilesTrackingService
    {
        Task<int> ProcessingAndUploadImageFileAsync(string userId, string localPath, IUnitOfWork unitOfWork, IFormFile imageFile);
        Task RemoveFileFromFileServerAsync(IUnitOfWork unitOfWork, List<int> fileIds);
        Task<List<FileTracking>> UploadMultipleFilesAsync(string userId, IUnitOfWork unitOfWork, List<FileTrackingRequestDto> requests);
        Task<FileTracking> UploadSingleFileAsync(string userId, IUnitOfWork unitOfWork, FileInfoRequestDto request, string uploadSource, string documentType);
        Task<List<string>> RemoveFileAsync(IUnitOfWork unitOfWork, List<int> fileIds);
    }
}
