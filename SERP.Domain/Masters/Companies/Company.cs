using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Companies
{
    public class Company : BaseModel
    {

        [StringLength(50)]
        public string company_no { get; set; }

        [StringLength(100)]
        public string company_name { get; set; }

        public int base_currency_id { get; set; }

        public int registered_site_id { get; set; }

        public int parent_group_id { get; set; }

        [StringLength(50)]
        public string company_registration_no { get; set; }

        [StringLength(50)]
        public string? gst_vat_registration_no { get; set; }

        [StringLength(50)]
        public string? tax_registration_no { get; set; }

        public bool intercompany_flag { get; set; }

        public bool dormant_flag { get; set; }

        [StringLength(1)]
        [AcceptValue(
        DomainConstant.StatusFlag.Enabled,
        DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; } = DomainConstant.StatusFlag.Enabled;
    }
}
