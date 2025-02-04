namespace SERP.Domain.Masters.Suppliers.Models
{
    public class PagedFilterSupplierRequestModel
    {
        public string? Keyword { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public bool? service_flag { get; set; }
        public bool? product_flag { get; set; }
        public HashSet<string>? status_flag { get; set; }
    }
}
