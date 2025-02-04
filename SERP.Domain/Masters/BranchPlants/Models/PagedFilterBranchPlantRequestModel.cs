using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Domain.Masters.BranchPlants.Models
{
    public class PagedFilterBranchPlantRequestModel
    {
        public string? Keyword { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<int> company_id { get; set; }
        public HashSet<string>? status_flag { get; set; }
    }
}
