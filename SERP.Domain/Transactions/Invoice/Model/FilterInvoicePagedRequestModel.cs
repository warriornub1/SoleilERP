namespace SERP.Domain.Transactions.Invoice.Model
{
    public class FilterInvoicePagedRequestModel
    {
        public string? Keyword { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<string>? status { get; set; }
        public HashSet<int>? branch_plant_id { get; set; }
        public HashSet<int>? supplier_id { get; set; }
    }
}
