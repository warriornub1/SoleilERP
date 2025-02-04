namespace SERP.Application.Transactions.CustomViews.DTOs.Request
{
    public class GetByCustomViewTypeRequestDto
    {
        public string custom_view_type { get; set; }
        public string user_id { get; set; }
        public bool private_flag { get; set; }
    }
}
