using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class CreateCustomViewRequestModel
    {
        [Required]
        [StringLength(20)]
        public string custom_view_type { get; set; }
        [Required]
        [StringLength(30)]
        public string custom_view_name { get; set; }
        public bool default_flag { get; set; }
        public bool private_flag { get; set; }
        [Required]
        [StringLength(1)]
        [AcceptValue("E", "D")]
        public string status_flag { get; set; }

        public bool allow_update_delete_flag { get; set; } = true;
        [StringLength(50)]
        public string? user_id { get; set; }
        public List<CreateAttributeViewModel> attributes { get; set; }
        public List<CreateCustomViewFilterModel>? filters { get; set; }
    }

    public class CreateAttributeViewModel : AttributeViewModel;

    public class CreateCustomViewFilterModel : CustomViewFilterRequestModel
    {
        [Obsolete]
        public new int id { get; set; }
    };
}
