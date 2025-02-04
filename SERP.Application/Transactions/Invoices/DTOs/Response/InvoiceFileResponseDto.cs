namespace SERP.Application.Transactions.Invoices.DTOs.Response
{
    public class InvoiceFileResponseDto
    {
        public int id { get; set; }
        public string file_name { get; set; }
        public string url_path { get; set; }
        public string document_type { get; set; }
        public string file_type { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
    }
}
