using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.InboundShipments.Model
{
    public class UploadIhsRequestModel
    {
        [Required]
        public int inbound_shipment_id { get; set; }
        [StringLength(100)]
        public string? upload_source { get; set; }
        [MaxFileCount(10)]
        public List<IFormFile> files { get; set; }
        [Required] 
        public string document_type { get; set; }
    }
}
