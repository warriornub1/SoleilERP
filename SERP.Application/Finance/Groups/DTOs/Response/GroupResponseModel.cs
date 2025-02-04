namespace SERP.Application.Finance.Groups.DTOs.Response
{
    public class GroupResponseModel
    {
        public int id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string group_type { get; set; }
        public int level { get; set; }
        public int? parent_group_id { get; set; }
        public string status_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
