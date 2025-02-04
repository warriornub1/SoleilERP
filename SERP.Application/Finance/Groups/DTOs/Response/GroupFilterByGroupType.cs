namespace SERP.Application.Finance.Groups.DTOs.Response
{
    public class GroupFilterByGroupType
    {
        public List<GroupFilter> items { get; set; }
    }

    public class GroupFilter
    {
        public int id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public int level { get; set; }
    }
}
