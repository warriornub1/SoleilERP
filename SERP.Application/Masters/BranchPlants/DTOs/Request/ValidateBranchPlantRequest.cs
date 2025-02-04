using SERP.Domain.Common.Enums;

namespace SERP.Application.Masters.BranchPlants.DTOs.Request
{
    public class ValidateBranchPlantRequest
    {
        public int company_id { get; set; }
        public int site_id { get; set; }
        public string branch_plant_no { get; set; }
        public SERPEnum.Mode mode { get; set; }
    }
}
