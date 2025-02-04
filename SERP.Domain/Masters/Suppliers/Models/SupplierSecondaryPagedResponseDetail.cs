namespace SERP.Domain.Masters.Suppliers.Models
{
    public class SupplierSecondaryPagedResponseDetail
    {
        public int secondary_supplier_id { get; set; }
        public string secondary_supplier_no { get; set; }
        public string secondary_supplier_name { get; set; }
        public string status_flag { get; set; }
        public bool default_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
