using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class SearchPoDetailRequestDto
    {
        public bool AllowZeroOpenQty { get; set; } = false;
        [AcceptValue(
            DomainConstant.PurchaseOrder.LineType.Item,
            DomainConstant.PurchaseOrder.LineType.Service,
            DomainConstant.PurchaseOrder.LineType.AirFreight,
            DomainConstant.PurchaseOrder.LineType.SeaFreight,
            DomainConstant.PurchaseOrder.LineType.LandFreight,
            DomainConstant.PurchaseOrder.LineType.MiscCharges
        )]
        public string? line_type { get; set; }
        public List<int>? PoHeaderIDs { get; set; }
        public List<int>? Suppliers { get; set; }
        public List<int>? BranchPlants { get; set; }
        public HashSet<string>? Statuses { get; set; }
    }
}
