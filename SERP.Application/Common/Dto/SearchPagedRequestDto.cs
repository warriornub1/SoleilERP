namespace SERP.Application.Common.Dto
{
    public class SearchPagedRequestDto: PagedRequestDto
    {
        public string? Keyword { get; set; } = string.Empty;
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }
}
