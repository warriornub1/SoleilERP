using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model;

public class UploadAsnFileRequestModel
{
    [Required]
    public int asn_header_id { get; set; }
    [StringLength(100)]
    public string? upload_source { get; set; }
    [MaxFileCount(10)]
    public List<AsnAttachment> attachments { get; set; }
}

public class AsnAttachment
{
    public IFormFile File { get; set; }
    public string document_type { get; set; }
}