using SERP.Domain.Masters.BranchPlants.Models.Base;

namespace SERP.Domain.Masters.BranchPlants.Models
{
    public class UpdateBranchPlantRequestModel: BranchPlantRequestModel
    {
        public int id { get; set; }
        [Obsolete]
        public new string? branch_plant_no { get; set; }
    }
}
