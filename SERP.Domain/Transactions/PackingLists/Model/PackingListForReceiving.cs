using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common;

namespace SERP.Domain.Transactions.PackingLists.Model
{
    public class PackingListForReceiving : BaseModel
    {
        [StringLength(50)]
        public string? package_no { get; set; }
        public int item_id { get; set; }
        public int packing_header_id { get; set; }
        public int? po_detail_id { get; set; }
        public int? asn_detail_id { get; set; }
        public int qty { get; set; }
        [StringLength(5)]
        public string uom { get; set; }
        [StringLength(5)]
        public string po_uom { get; set; }
        public int? country_of_origin_id { get; set; }
    }
}
