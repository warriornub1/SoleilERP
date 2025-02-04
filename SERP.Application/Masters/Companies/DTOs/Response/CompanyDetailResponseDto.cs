namespace SERP.Application.Masters.Companies.DTOs.Response
{
    public class CompanyDetailResponseDto
    {
        public int id { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set; }
        public int? base_currency_id { get; set; }
        public bool status_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
