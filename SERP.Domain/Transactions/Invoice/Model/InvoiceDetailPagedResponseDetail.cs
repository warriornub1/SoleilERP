using SERP.Domain.Masters.Countries.Models;

namespace SERP.Domain.Transactions.Invoice.Model
{
    public class InvoiceDetailPagedResponseDetail
    {
        public int detail_id { get; set; }
        public string invoice_no { get; set; }
        public int invoice_line_no { get; set; }
        public int invoice_header_id { get; set; }
        public string primary_uom { get; set; }
        public decimal po_unit_cost { get; set; }
        public string po_currency { get; set; }
        public string invoice_currency { get; set; }
        public string asn_no { get; set; }
        public string po_no { get; set; }
        public int po_line_no { get; set; }
        public string status_flag { get; set; }
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string supplier_part_no { get; set; }
        public int qty { get; set; }
        public string uom { get; set; }
        public CountryBasicDetail country_of_origin { get; set; }
        public decimal? exchange_rate { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_price { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
