using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.LOVs
{
    public class Lov : BaseModel
    {
        [StringLength(50)]
        public string lov_type { get; set; }

        [StringLength(50)]
        public string lov_value { get; set; }

        [StringLength(100)]
        public string lov_label { get; set; }

        [StringLength(255)]
        public string? extended_data_1 { get; set; }

        [StringLength(255)]
        public string? extended_data_2 { get; set; }

        [StringLength(255)]
        public string? description { get; set; }

        [StringLength(1)]
        [AcceptValue(
        DomainConstant.StatusFlag.Enabled,
        DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; }

        public bool default_flag { get; set; }
    }
}
