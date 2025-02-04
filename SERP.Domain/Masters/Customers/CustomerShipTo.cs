using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Customers
{
    public class CustomerShipTo : BaseModel
    {
        public int customer_id { get; set; }

        public int site_id { get; set; }

        public bool default_flag { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }

    }
}
