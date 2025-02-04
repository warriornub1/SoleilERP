using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Suppliers
{
    public class SupplierItemMapping : BaseModel
    {
        public int supplier_id { get; set; }

        public int item_id { get; set; }

        [StringLength(100)]
        public string? supplier_part_no { get; set; }

        [StringLength(50)]
        public string? supplier_material_code { get; set; }

        [StringLength(255)]
        public string? supplier_material_description { get; set; }

        public bool default_flag { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
