namespace SERP.Domain.Common.Model
{
    public class SearchPagedRequestModel: PagedRequestModel
    {
        public string? Keyword { get; set; } = string.Empty;
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
