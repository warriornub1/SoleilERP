using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.BranchPlants.Models.Base
{
    public class BranchPlantRequestModel
    {
        [Required]
        public int company_id { get; set; }
        [Required]
        public int site_id { get; set; }
        [StringLength(50)]
        public string branch_plant_no { get; set; }
        [StringLength(100)]
        public string branch_plant_name { get; set; }
        [Required]
        [AcceptValue(
            DomainConstant.StatusFlag.Enabled,
            DomainConstant.StatusFlag.Disabled
            )]
        public string status_flag { get; set; }
    }
}
