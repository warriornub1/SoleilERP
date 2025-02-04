namespace SERP.Application.Finance.Natural_Accounts.DTOs.Request
{
    public class NaturalAccountBaseDto
    {
        public string natural_account_code { get; set; }
        public string natural_account_description { get; set; }
        public string natural_account_type { get; set; }
        public int parent_group_id { get; set; }
        public string status_flag { get; set; }
    }
}
