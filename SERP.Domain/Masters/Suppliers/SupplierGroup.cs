using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Suppliers
{
    public class SupplierGroup : BaseModel
    {
        [StringLength(50)]
        public string group_supplier_no { get; set; }
        [StringLength(100)]
        public string group_supplier_name { get; set; }
        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
