namespace SERP.Application.Masters.BranchPlants.DTOs.Request
{
    public class CreateBranchPlantRequestDto
    {
        public int company_id { get; set; }
        public int site_id { get; set; }
        public string branch_plant_no { get; set; }
        public string branch_plant_name { get; set; }
        public string status_flag { get; set; }
    }
}
