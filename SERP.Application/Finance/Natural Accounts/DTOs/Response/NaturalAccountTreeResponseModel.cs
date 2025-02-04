using SERP.Application.Finance.CostCenters.DTOs.Response;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Response
{
    public class NaturalAccountTreeResponseModel
    {
        public int group_id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string status_flag { get; set; }
        public int level { get; set; }
        public List<NaturalAccountList>? natural_account_list { get; set; }
        public List<NaturalAccountTreeResponseModel> child_group_list { get; set; }
    }

    public class NaturalAccountList
    {
        public int id { get; set; }
        public string natural_account_code { get; set; }
        public string natural_account_description { get; set; }
        public string status_flag { get; set; }
    }
}
