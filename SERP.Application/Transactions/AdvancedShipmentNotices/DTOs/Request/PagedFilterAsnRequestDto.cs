using SERP.Application.Common.Dto;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class PagedFilterAsnRequestDto: SearchPagedRequestDto
    {
        public string bpNo { get; set; }
        public int asn_header_id { get; set; }
        public HashSet<int>? suppliers { get; set; }
        public HashSet<string>? Statuses { get; set; }
        public HashSet<int>? PoHeaderIDs { get; set; }
        public List<int>? Items { get; set; }
        public List<int>? BranchPlants { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
    }
}
