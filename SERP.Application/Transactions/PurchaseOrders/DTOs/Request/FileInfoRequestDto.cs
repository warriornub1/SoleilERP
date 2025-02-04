using Microsoft.AspNetCore.Http;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class FileInfoRequestDto
    {
        public IFormFile file { get; set; }
        public string url_path { get; set; }
        public string document_type { get; set; }
    }
}
