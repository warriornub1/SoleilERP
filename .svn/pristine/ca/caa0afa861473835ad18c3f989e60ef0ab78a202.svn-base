using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Items
{
    public class Item : BaseModel
    {
        [StringLength(50)]
        public string item_no { get; set; }

        [StringLength(255)]
        public string description_1 { get; set; }

        [StringLength(255)]
        public string? description_2 { get; set; }

        [StringLength(50)]
        public string? brand { get; set; }

        [StringLength(5)]
        public string primary_uom { get; set; }

        [StringLength(5)]
        public string secondary_uom { get; set; }

        [StringLength(5)]
        public string purchasing_uom { get; set; }

        public int? item_uom_conversion_id { get; set; }

        public int purchase_min_order_qty { get; set; }

        public int purchase_multiple_order_qty { get; set; }

        public bool label_required_flag { get; set; }

        public bool lot_control_flag { get; set; }

        [StringLength(1024)]
        public string? inspection_instruction { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }
    }
}
