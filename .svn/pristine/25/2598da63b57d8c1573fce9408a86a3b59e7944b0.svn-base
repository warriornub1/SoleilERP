using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Ports
{
    public class Port : BaseModel
    {
        [StringLength(50)]
        public string port_no { get; set; }

        [StringLength(100)]
        public string port_name { get; set; }

        public int? country_id { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
