namespace SERP.Application.Finance.Groups.DTOs
{
    public class GroupExcelDto
    {
        public string? code { get; set; }
        public string? description { get; set; }
        public string groupType { get; set; }
        public int? level { get; set; }
        public string? parentGroup { get; set; }
        public string? status { get; set; }
    }
}
