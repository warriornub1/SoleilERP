namespace SERP.Domain.Common.Model
{
    public class PagedResponseModel<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
    }
}
