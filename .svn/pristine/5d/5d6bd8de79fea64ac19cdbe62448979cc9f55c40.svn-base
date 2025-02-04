using SERP.Domain.Masters.Companies.Models;
using SERP.Domain.Masters.Countries.Models;

namespace SERP.Domain.Masters.BranchPlants.Models
{
    public class BranchPlantDetail
    {
        public int id { get; set; }
        public string branch_plant_no { get; set; }
        public string branch_plant_name { get; set; }
        public CompanyDetail company { get; set; }
        public string status_flag { get; set; }
        public BranchPlantDetailSite? site { get; set; }
    }

    public class BranchPlantDetailSite
    {
        public int site_id { get; set; }
        public string site_no { get; set; }
        public string site_name { get; set; }
        public string? address_line_1 { get; set; }
        public string? address_line_2 { get; set; }
        public string? address_line_3 { get; set; }
        public string? address_line_4 { get; set; }
        public string? postal_code { get; set; }
        public string? state_province { get; set; }
        public string? county { get; set; }
        public string? city { get; set; }
        public CountryDetail country { get; set; }
    }
}
