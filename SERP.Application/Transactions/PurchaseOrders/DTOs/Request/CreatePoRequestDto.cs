using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SERP.Domain.Common.Attributes;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class CreatePoRequestDto
    {
        public string action { get; set; }
        public List<CreatePoInfoRequestDto> pos { get; set; }
        [StringLength(100)]
        public string upload_source { get; set; }
        [MaxFileCount(10)]
        public List<IFormFile>? files { get; set; }
    }

    public class CreatePoInfoRequestDto
    {
        public CreatePoHeaderRequestDto po_header { get; set; }
        public List<CreatePoDetailRequestDto>? po_detail { get; set; }
    }
}
