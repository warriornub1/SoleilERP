using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Finance.Groups
{
    public class Group : BaseModel
    {
        [StringLength(6)]
        public string group_code { get; set; }

        [StringLength(255)]
        public string group_description { get; set; }

        [StringLength(2)]
        [AcceptValue(
        DomainConstant.Group.GroupType.Company,
        DomainConstant.Group.GroupType.NaturalAccount,
        DomainConstant.Group.GroupType.CostCenter,
        DomainConstant.Group.GroupType.RevenueCenter
        )]
        public string group_type { get; set; }
        public int level { get; set; }
        public int? parent_group_id { get; set; }

        [StringLength(1)]
        [AcceptValue(
        DomainConstant.StatusFlag.Enabled,
        DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; } = DomainConstant.StatusFlag.Enabled;
    }
}
