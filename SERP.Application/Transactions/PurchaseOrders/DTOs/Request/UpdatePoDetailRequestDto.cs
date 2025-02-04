using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class UpdatePoDetailRequestDto : CreatePoDetailRequestDto
    {
        public int id { get; set; }
        [AcceptValue(
            DomainConstant.PurchaseOrder.StatusFlag.Draft,
            DomainConstant.PurchaseOrder.StatusFlag.New,
            DomainConstant.PurchaseOrder.StatusFlag.InProcess,
            DomainConstant.PurchaseOrder.StatusFlag.Closed,
            DomainConstant.PurchaseOrder.StatusFlag.Cancelled)]
        public new string status_flag { get; set; }
    }
}
