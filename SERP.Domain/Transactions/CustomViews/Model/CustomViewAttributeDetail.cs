namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class CustomViewAttributeDetail
    {
        public int custom_view_id { get; set; }
        public string custom_view_name { get; set; }
        public List<AttributeDetail> attributes { get; set; }
        public List<CustomViewFilterDetail> filters { get; set; }
    }

    public class AttributeDetail
    {
        public int custom_view_attribute_id { get; set; }
        public string attribute { get; set; }
        public string attribute_type { get; set; }
        public int seq_no { get; set; }
        public bool column_freeze_flag { get; set; }
        public string? sort_direction { get; set; }
    }
}
