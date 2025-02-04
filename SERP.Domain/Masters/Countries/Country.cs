using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Countries
{
    public class Country : BaseModel
    {

        [StringLength(50)]
        public string country_name { get; set; }

        [StringLength(2)]
        public string country_alpha_code_two { get; set; }

        [StringLength(3)]
        public string country_alpha_code_three { get; set; }

        [StringLength(255)]
        public string? country_long_name { get; set; }

        [StringLength(3)]
        public string country_idd { get; set; }

        [StringLength(25)]
        public string continent { get; set; }
    }
}
