using Microsoft.AspNetCore.Http;

namespace SERP.Application.Transactions.FilesTracking.DTOs
{
    public class FileUploadBaseRequestDto
    {
        public string file_type { get; set; }
        public string document_type { get; set; }
        public string upload_source { get; set; }
        public IFormFile file { get; set; }
    }
}
