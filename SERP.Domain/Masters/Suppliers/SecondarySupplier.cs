using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Suppliers
{
    public class SecondarySupplier : BaseModel
    {
        public int supplier_id { get; set; }

        [StringLength(50)]
        public string sec_supplier_no { get; set; }

        [StringLength(100)]
        public string sec_supplier_name { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }

        public bool default_flag { get; set; }
    }
}
