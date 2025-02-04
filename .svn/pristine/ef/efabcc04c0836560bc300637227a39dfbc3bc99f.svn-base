using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Transactions.CustomViews.DTOs.Request
{
    public class UpdateCustomViewRequestDto
    {
        public int id { get; set; }
        [Required]
        [StringLength(20)]
        public string custom_view_type { get; set; }
        [Required]
        [StringLength(30)]
        public string custom_view_name { get; set; }
        public bool default_flag { get; set; }
        public bool private_flag { get; set; }
        [AcceptValue("E", "D")]
        [StringLength(1)]
        public string status_flag { get; set; }
        public bool allow_update_delete_flag { get; set; } = true;
        [StringLength(50)]
        public string? user_id { get; set; }
    }
}
