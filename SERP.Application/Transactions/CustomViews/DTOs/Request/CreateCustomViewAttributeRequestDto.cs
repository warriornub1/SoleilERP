namespace SERP.Application.Transactions.CustomViews.DTOs.Request
{
    public class CreateCustomViewAttributeRequestDto
    {
        public string attribute { get; set; }
        public string attribute_type { get; set; }
        public int seq_no { get; set; }
        public bool column_freeze_flag { get; set; }
        public string? sort_direction { get; set; }
    }
}

