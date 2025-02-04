using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.BranchPlants
{
    public class BranchPlant : BaseModel
    {
        public int company_id { get; set; }

        public int site_id { get; set; }

        [StringLength(50)]
        public string branch_plant_no { get; set; }

        [StringLength(100)]
        public string branch_plant_name { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
