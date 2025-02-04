namespace SERP.Application.Masters.Companies.DTOs.Response
{
    public class CompanyTreeResponseDto
    {
        public int group_id { get; set; }
        public string group_code { get; set; }
        public string group_description { get; set; }
        public string status_flag { get; set; }
        public int level { get; set; }
        public List<CompanyList>? company_list { get; set; }
        public List<CompanyTreeResponseDto> child_group_list { get; set; }
    }

    public class CompanyList
    {
        public int id { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set; }
        public string status_flag { get; set; }
    }
}
