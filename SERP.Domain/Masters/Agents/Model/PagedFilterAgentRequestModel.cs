namespace SERP.Domain.Masters.Agents.Model
{
    public class PagedFilterAgentRequestModel
    {
        public string? Keyword { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<string>? agent_type { get; set; }
        public HashSet<string>? status_flag { get; set; }
    }
}
