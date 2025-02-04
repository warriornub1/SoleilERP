namespace SERP.Application.Finance.RevenueCenters.DTOs.Response
{
    public class RevenueCenterTreeResponseModel
    {
        public int group_id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string status_flag { get; set; }
        public int level { get; set; }
        public List<RevenueCenterList>? revenue_center_list { get; set; }
        public List<RevenueCenterTreeResponseModel> child_group_list { get; set; }
    }

    public class RevenueCenterList
    {
        public int id { get; set; }
        public string revenue_center_code { get; set; }
        public string revenue_center_description { get; set; }
        public string status_flag { get; set; }
    }
}
