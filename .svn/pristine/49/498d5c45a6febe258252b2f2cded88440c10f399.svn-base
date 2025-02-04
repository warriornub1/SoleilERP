using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews.Model
{
    public class AttributeViewModel
    {
        [StringLength(255)]
        public string attribute { get; set; }
        /// <summary>
        /// 01: Text
        /// 02: Numeric
        /// 03: Date
        /// 04: Date/Time
        /// </summary>
        [StringLength(2)]
        public string attribute_type { get; set; }
        [Required]
        public int seq_no { get; set; }
        public bool column_freeze_flag { get; set; }
        public string? sort_direction { get; set; }
    }
}
