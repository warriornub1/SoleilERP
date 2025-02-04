namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base
{
    public class FilterAsnDetailRequestDto
    {
        public HashSet<int>? po_header_id { get; set; }
        public HashSet<int>? asn_header_id { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<string>? status { get; set; }

    }
}
