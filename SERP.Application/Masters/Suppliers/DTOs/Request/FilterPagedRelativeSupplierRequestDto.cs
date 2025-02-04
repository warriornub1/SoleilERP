namespace SERP.Application.Masters.Suppliers.DTOs.Request
{
    public class FilterPagedRelativeSupplierRequestDto
    {
        public int supplier_id { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public bool? default_flag { get; set; }
        public HashSet<string>? status_flag { get; set; }
    }
}
