using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Masters.Suppliers.DTOs
{
    public class SecondarySupplierLimitedDto
    {
        public int secondary_supplier_id { get; set; }
        public string secondary_supplier_no { get; set; }
        public string secondary_supplier_name { get; set; }
        public string secondary_supplier_status_flag { get; set; }
    }
}
