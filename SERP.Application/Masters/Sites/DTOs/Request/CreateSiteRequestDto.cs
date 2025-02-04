namespace SERP.Application.Masters.Sites.DTOs.Request
{
    public class CreateSiteRequestDto
    {
        public string site_no { get; set; }
        public string site_name { get; set; }
        public string? address_line_1 { get; set; }
        public string? address_line_2 { get; set; }
        public string? address_line_3 { get; set; }
        public string? address_line_4 { get; set; }
        public string? city { get; set; }
        public string? state_province { get; set; }
        public string? postal_code { get; set; }
        public string? county { get; set; }
        public int? country_id { get; set; }
    }
}
