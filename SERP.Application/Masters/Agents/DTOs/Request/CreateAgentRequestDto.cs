using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Masters.Agents.DTOs.Request
{
    public class CreateAgentRequestDto
    {
        [Required]
        [StringLength(50)]
        public string agent_no { get; set; }
        [Required]
        [StringLength(100)]
        public string agent_name { get; set; }
        [Required]
        public string agent_type { get; set; }
        [Required]
        [AcceptValue(
            DomainConstant.StatusFlag.Enabled,
            DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; }
    }
}
