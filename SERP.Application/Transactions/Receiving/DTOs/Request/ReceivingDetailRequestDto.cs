namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class ReceivingDetailRequestDto
    {
        public int? po_detail_id { get; set; }
        public int? asn_detail_id { get; set; }
        public int? packing_header_id { get; set; }
        public int qty { get; set; }
        public string? package_no { get; set; }
        public int? country_of_origin_id { get; set; }
        public string? supplier_document_type { get; set; }
        public string? supplier_document_no { get; set; }
    }
}
