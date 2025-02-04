namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class CustomViewDetail
    {
        public int id { get; set; }
        public string custom_view_type { get; set; }
        public string custom_view_name { get; set; }
        public bool private_flag { get; set; }
        public bool allow_update_delete_flag { get; set; }
        public bool default_flag { get; set; }
        public bool status_flag { get; set; }
    }
}
