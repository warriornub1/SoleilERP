using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class UpdatePoHeaderRequestDto : CreatePoHeaderRequestDto
    {
        public int id { get; set; }
        [Obsolete("Not allow to update supplier", true)]
        public new int supplier_id { get; set; }
        [Required]
        [AcceptValue(
            DomainConstant.PurchaseOrder.StatusFlag.Draft,
            DomainConstant.PurchaseOrder.StatusFlag.New,
            DomainConstant.PurchaseOrder.StatusFlag.InProcess,
            DomainConstant.PurchaseOrder.StatusFlag.Closed,
            DomainConstant.PurchaseOrder.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }
}
