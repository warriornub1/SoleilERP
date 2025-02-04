using Microsoft.AspNetCore.Http;

namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class UploadReceivingFileDto
    {
        public int receiving_header_id { get; set; }
        public int receiving_detail_id { get; set; }

        public List<IFormFile> files { get; set; }
    }
}
