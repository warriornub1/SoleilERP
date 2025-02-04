namespace SERP.Domain.Masters.Suppliers.Models
{
    public class SecondarySupplierPagedResponseDetail
    {
        public int seconday_supplier_id { get; set; }
        public string seconday_supplier_no { get; set; }
        public string seconday_supplier_name { get; set; }
        public string seconday_supplier_status_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
