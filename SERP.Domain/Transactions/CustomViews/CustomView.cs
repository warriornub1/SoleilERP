using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews
{
    public class CustomView : BaseModel
    {
        /// <summary>
        /// Indicating which list it belongs to for example PO List, Sales Order List, ASN List, etc
        /// </summary>
        [StringLength(20)]
        public string custom_view_type { get; set; }

        [StringLength(30)]
        public string custom_view_name { get; set; }

        /// <summary>
        /// Default attribute list for Custom View. Needed for each Custom View Type
        /// </summary>
        public bool default_flag { get; set; }

        /// <summary>
        /// Indicate if the user is allowed to delete the view
        /// </summary>
        public bool allow_update_delete_flag { get; set; }

        /// <summary>
        /// Indicate if the view is private to a specific user only.
        /// </summary>
        public bool private_flag { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }

        /// <summary>
        /// If private_flag is 1: True, indicate user_id
        /// </summary>
        [StringLength(50)]
        public string? user_id { get; set; }
    }
}
