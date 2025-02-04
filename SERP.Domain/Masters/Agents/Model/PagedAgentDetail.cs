namespace SERP.Domain.Masters.Agents.Model
{
    public class PagedAgentDetail
    {
        public int id { get; set; }
        public string agent_no { get; set; }
        public string agent_name { get; set; }
        public string agent_type { get; set; }
        public string status_flag { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string? last_modified_by { get; set; }
        public DateTime? last_modified_on { get; set; }
    }
}
