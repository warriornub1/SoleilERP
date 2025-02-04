using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Sites
{
    public class Site : BaseModel
    {

        [StringLength(50)]
        public string site_no { get; set; }

        [StringLength(255)]
        public string site_name { get; set; }

        [StringLength(255)]
        public string? address_line_1 { get; set; }

        [StringLength(255)]
        public string? address_line_2 { get; set; }

        [StringLength(255)]
        public string?  address_line_3 { get; set; }

        [StringLength(255)]
        public string? address_line_4 { get; set; }

        [StringLength(20)]
        public string? postal_code { get; set; }

        public int? country_id { get; set; }

        [StringLength(50)]
        public string? state_province { get; set; }

        [StringLength(50)]
        public string? county { get; set; }

        [StringLength(50)]
        public string? city { get; set; }
    }
}
