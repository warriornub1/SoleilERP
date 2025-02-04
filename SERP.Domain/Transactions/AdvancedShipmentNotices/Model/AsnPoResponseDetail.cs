namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class AsnPoResponseDetail
    {
        public int po_header_id { get; set; }
        public int po_detail_id { get; set; }
        public string po_no { get; set; }
        public int po_line_no { get; set; }
        public decimal po_open_qty { get; set; }
        public string po_currency { get; set; }
        public decimal po_unit_cost { get; set; }
    }
}
