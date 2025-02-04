using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Finance.CostCenters
{
    public class CostCenter : BaseModel
    {
        [StringLength(DomainDBLength.CostCenter.CostCenterCode)]
        public string cost_center_code { get; set; }

        [StringLength(DomainDBLength.CostCenter.CostCenterDescription)]
        public string cost_center_description { get; set; }
        public int parent_group_id { get; set; }
        public int? company_structure_id { get; set; }

        [StringLength(DomainDBLength.CostCenter.StatusFlag)]
        [AcceptValue(
        DomainConstant.StatusFlag.Enabled,
        DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; } = DomainConstant.StatusFlag.Enabled;
    }
}
