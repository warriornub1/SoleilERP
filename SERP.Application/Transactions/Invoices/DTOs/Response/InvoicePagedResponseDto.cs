namespace SERP.Application.Transactions.Invoices.DTOs.Response
{
    public class InvoicePagedResponseDto
    {
        public int id { get; set; }
        public string invoice_no { get; set; }
        public string? asn_no { get; set; }
        public string? receiving_no { get; set; }
        public string status_flag { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public string? branch_plant_no { get; set; }
        public string? branch_plant_name { get; set; }
        public string currency { get; set; }
        public decimal amt { get; set; }
        public decimal total_amt { get; set; }
        public string? variance_reason { get; set; }
        public DateOnly? invoice_date { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
