using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Suppliers
{
    public class SupplierSelfCollectSite : BaseModel
    {
        /// <summary>
        /// Supplier ID	Foreign key from Supplier Master
        /// </summary>
        public int supplier_id { get; set; }
        /// <summary>
        /// Site ID	Foreign key from Site Master
        /// </summary>
        public int site_id { get; set; }
        /// <summary>
        /// status_flag
        /// E: Enabled - D: Disabled
        /// </summary>
        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
