using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews
{
    public class CustomViewAttribute : BaseModel
    {
        public int custom_view_id { get; set; }

        /// <summary>
        /// Attribute name for example po_no, requested_date, etc
        /// </summary>
        [StringLength(255)]
        public string attribute { get; set; }

        /// <summary>
        /// Type of attribute.
        /// 01: Text
        /// 02: Numeric
        /// 03: Date
        /// 04: Date/Time
        /// </summary>
        public string attribute_type { get; set; }

        /// <summary>
        /// Indicating the sequence of column to be displayed.
        /// </summary>
        public int seq_no { get; set; }

        /// <summary>
        /// Indicating that the column and before this column will be freeze.
        /// 0: False
        /// 1: True
        /// </summary>
        public bool column_freeze_flag { get; set; }

        /// <summary>
        /// Indicating that the column sorting by ascending or descending or none. If none then it will be blank.
        /// "A: Ascending D: Descending"
        /// </summary>
        [StringLength(1)]
        public string? sort_direction { get; set; }
    }
}
