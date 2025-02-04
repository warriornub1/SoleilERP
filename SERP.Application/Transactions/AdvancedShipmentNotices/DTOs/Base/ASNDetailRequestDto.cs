namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base
{
    public class ASNDetailRequestDto
    {
        public string status_flag { get; set; }
        public int line_no { get; set; }
        public int item_id { get; set; }
        public int qty { get; set; }
        public string  uom { get; set; }
        public decimal unit_cost { get; set; }
        public int? country_of_origin_id { get; set; }
        public string? notes_to_warehouse { get; set; }
        public int po_detail_id { get; set; }
    }
}
