using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Masters.Suppliers.DTOs
{
    public class SecondarySupplierDto
    {
        public int id { get; set; }
        public string supplier_no { get; set; }
        public string status_flag { get; set; }
        public string supplier_name { get; set; }
    }
}
