namespace SERP.Domain.Common.Model
{
    public class PagedResponse<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
