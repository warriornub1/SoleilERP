using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Customers
{
    public class Customer : BaseModel
    {
        [StringLength(50)]
        public string customer_no { get; set; }

        [StringLength(100)]
        public string customer_name { get; set; }

        public int bill_to_site_id { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
