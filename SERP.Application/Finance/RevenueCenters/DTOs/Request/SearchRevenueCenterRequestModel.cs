
namespace SERP.Application.Finance.RevenueCenters.DTOs.Request
{
    public class SearchRevenueCenterRequestModel
    {
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public List<ParentGroupListRC>? parentGroupList { get; set; }
        public List<StatusListRC>? statusList { get; set; }
    }

    public class ParentGroupListRC
    {
        public int group_id { get; set; }
    }

    public class StatusListRC
    {
        public string status { get; set; }
    }
}
