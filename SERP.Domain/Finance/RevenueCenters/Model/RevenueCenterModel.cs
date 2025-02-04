namespace SERP.Domain.Finance.RevenueCenters.Model
{
    public class RevenueCenterModel
    {
        public int id { get; set; }
        public string revenue_center_code { get; set; }
        public string revenue_center_description { get; set; }
        public int parent_group_id { get; set; }
        public string status_flag { get; set; }
        public int? company_structure_id { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

}
