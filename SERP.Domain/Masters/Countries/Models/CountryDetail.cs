namespace SERP.Domain.Masters.Countries.Models
{
    public class CountryDetail
    {
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string? country_long_name { get; set; }
        public string country_alpha_code_two { get; set; }
        public string country_alpha_code_three { get; set; }
        public string country_idd { get; set; }
        public string continent { get; set; }
    }
}
