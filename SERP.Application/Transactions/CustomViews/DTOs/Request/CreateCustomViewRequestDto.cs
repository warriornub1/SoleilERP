namespace SERP.Application.Transactions.CustomViews.DTOs.Request
{
    public class CreateCustomViewRequestDto
    {
        public string custom_view_type { get; set; }
        public string custom_view_name { get; set; }
        public bool default_flag { get; set; }
        public bool private_flag { get; set; }
        public string status_flag { get; set; }
        public string user_id { get; set; }
        public bool allow_update_delete_flag { get; set; } = true;
        public List<CreateCustomViewAttributeRequestDto> attributes { get; set; }
        public List<CustomViewFilterRequestDto>? filters { get; set; }

    }
}
