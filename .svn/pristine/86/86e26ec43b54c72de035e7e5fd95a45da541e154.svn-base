
using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.PurchaseOrders.Model;

public class UploadFileRequestModel
{
    [Required]
    public int po_header_id { get; set; }
    [StringLength(100)]
    public string? upload_source { get; set; }
    [MaxFileCount(10)]
    public List<IFormFile> files { get; set; }
}