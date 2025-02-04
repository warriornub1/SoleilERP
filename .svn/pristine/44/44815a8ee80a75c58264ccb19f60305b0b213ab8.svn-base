using Microsoft.AspNetCore.Http;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Transactions.Invoices.DTOs.Request
{
    public class UploadInvoiceFileRequestDto
    {
        public int invoice_header_id { get; set; }
        public string upload_source { get; set; }
        public string document_type { get; set; }

        public List<FileInfoRequestDto> files { get; set; }
    }

    public class UploadInvoiceRequestModel
    {
        [Required]
        public int invoice_header_id { get; set; }
        [StringLength(100)]
        public string? upload_source { get; set; }
        [MaxFileCount(10)]
        public List<IFormFile> files { get; set; }
        [Required] 
        public string document_type { get; set; }
    }
}
