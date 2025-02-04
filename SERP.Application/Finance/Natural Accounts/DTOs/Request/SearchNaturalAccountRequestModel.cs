using SERP.Application.Finance.RevenueCenters.DTOs.Request;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Request
{
    public class SearchNaturalAccountRequestModel
    {
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public List<ParentGroupListNA>? parentGroupList { get; set; }
        public List<TpyeListNA>? typeList { get; set; }
        public List<StatusListNA>? statusList { get; set; }
    }
    public class ParentGroupListNA
    {
        public int group_id { get; set; }
    }

    public class TpyeListNA
    {
        public string type { get; set; }
    }

    public class StatusListNA
    {
        public string status { get; set; }
    }
}
