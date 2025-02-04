using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model.Base
{
    public class ASNDetailViewModel
    {
        [Required]
        public string status_flag { get; set; }
        public int line_no { get; set; }
        public int item_id { get; set; }
        public int qty { get; set; }
        [DecimalPrecision(8,4)]
        public decimal unit_cost { get; set; }
        public int? country_of_origin_id { get; set; }
        [StringLength(255)]
        public string? notes_to_warehouse { get; set; }
        public int po_detail_id { get; set; }
    }
}
