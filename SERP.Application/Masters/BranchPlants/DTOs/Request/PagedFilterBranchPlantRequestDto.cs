using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Application.Masters.BranchPlants.DTOs.Request
{
    public class PagedFilterBranchPlantRequestDto
    {
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public HashSet<int> company_id { get; set; }
        public HashSet<string> status_flag { get; set; }
    }
}
