namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class PagedFilterCustomViewRequestModel
    {
        public string? Keyword { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<string>? custom_view_type { get; set; }
    }
}
