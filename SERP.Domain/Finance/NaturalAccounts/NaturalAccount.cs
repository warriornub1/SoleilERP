using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Finance.NaturalAccounts
{
    public class NaturalAccount : BaseModel
    {
        [StringLength(DomainDBLength.NaturalAccountDBLength.NaturalAccountCode)]
        public string natural_account_code { get; set; }

        [StringLength(DomainDBLength.NaturalAccountDBLength.NaturalAccountDescription)]
        public string natural_account_description { get; set; }

        [StringLength(DomainDBLength.NaturalAccountDBLength.NaturalAccountType)]
        [AcceptValue(
        DomainConstant.NaturalAccount.NaturalAccountType.Asset,
        DomainConstant.NaturalAccount.NaturalAccountType.Liability,
        DomainConstant.NaturalAccount.NaturalAccountType.Shareholders_Equity,
        DomainConstant.NaturalAccount.NaturalAccountType.PAndL
        )]
        public string natural_account_type { get; set; }
        public int parent_group_id { get; set; }

        [StringLength(DomainDBLength.NaturalAccountDBLength.StatusFlag)]
        [AcceptValue(
        DomainConstant.StatusFlag.Enabled,
        DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; } = DomainConstant.StatusFlag.Enabled;
    }
}
