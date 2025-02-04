using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Items
{
    public class ItemUomConversion : BaseModel
    {
        public int item_id { get; set; }

        [StringLength(5)]
        public string primary_uom { get; set; }

        [StringLength(5)]
        public string secondary_uom { get; set; }

        public int primary_uom_qty { get; set; }

        public int secondary_uom_qty { get; set; }
    }
}
