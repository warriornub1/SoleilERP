namespace SERP.Application.Common.Dto
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
