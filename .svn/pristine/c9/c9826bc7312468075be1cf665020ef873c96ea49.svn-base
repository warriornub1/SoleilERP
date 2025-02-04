using SERP.Domain.Common.Model;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class PagedFilterPoRequestDto : SearchPagedRequestModel
    {
        public bool AllowZeroOpenQty { get; set; } = false;
        public string branchPlantNo { get; set; }
        public List<int>? Suppliers { get; set; }
        public List<int>? PoHeaderIDs { get; set; }
        public List<int>? Items { get; set; }
        public List<int>? BranchPlants { get; set; }
        public List<string>? Statuses { get; set; }
        public string? line_type  { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
    }
}
