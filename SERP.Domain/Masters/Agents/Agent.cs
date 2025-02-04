using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Agents
{
    public class Agent : BaseModel
    {

        [StringLength(50)]
        public string agent_no { get; set; }

        [StringLength(100)]
        public string agent_name { get; set; }

        [StringLength(50)]
        public string agent_type { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
