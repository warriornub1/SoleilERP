namespace SERP.Application.Transactions.Invoices.DTOs.Response
{
    public class InvoiceDetailResponseDto
    {
        public int id { get; set; }
        //public string invoice_no { get; set; }
        public int invoice_line_no { get; set; }
        public string po_no { get; set; }
        public int item_id { get; set; }
        public int po_detail_id { get; set; }
        public int po_line_no { get; set; }
        //public string status_flag { get; set; }
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string supplier_part_no { get; set; }
        public int qty { get; set; }
        public string uom { get; set; }

        public int? country_of_origin_id { get; set; }
        //public CountryBasicResponseDto country_of_origin { get; set; }
        public decimal? exchange_rate { get; set; }
        //public string currency_code { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_price { get; set; }
        // po_unit_cost
        // po_currency_id
        // po_open_qty
        public decimal po_unit_cost { get; set; }
        public int po_currency_id { get; set; }
        public int po_open_qty { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
