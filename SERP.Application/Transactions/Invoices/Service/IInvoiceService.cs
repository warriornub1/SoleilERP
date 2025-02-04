using SERP.Application.Common.Dto;
using SERP.Application.Transactions.Invoices.DTOs.Request;
using SERP.Application.Transactions.Invoices.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.Invoices.Service
{
    public interface IInvoiceService
    {
        PagedResponse<InvoicePagedResponseDto> SearchPaged(SearchPagedRequestDto request, FilterInvoicePagedRequestDto filter);
        PagedResponse<InvoiceDetailPagedResponseDto> SearchDetail(SearchPagedRequestDto request, FilterInvoiceDetailRequestDto filter);
        Task<InvoiceInfoResponseDto> GetByIdAsync(int id);
        Task<InvoiceInfoResponseDto> GetByInvoiceNoAsync(string invoiceNo);
        Task<int[]> CreateInvoiceAsync(string userId, CreateInvoiceRequestDto request);
        Task UpdateInvoiceAsync(string userId, UpdateInvoiceRequestDto request);
        Task DeleteInvoiceAsync(string userId, int invoiceHeaderId);
        Task<int[]> UploadFileAsync(string userId, UploadInvoiceFileRequestDto request);
        Task<List<string>> RemoveFileAsync(int invoiceHeaderId, List<int> invoiceFileIDs);
    }
}
