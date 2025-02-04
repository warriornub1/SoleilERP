using SERP.Domain.Common;

namespace SERP.Domain.Transactions.PackingLists
{
    public class ASNPackingDiscrepancy: BaseModel
    {
        public int asn_header_id { get; set; }
        public int item_id { get; set; }
        public int? country_of_origin_id { get; set; }
        public int asn_qty { get; set; }
        public int packing_list_qty { get; set; }
        public string? uom { get; set; }
    }
}
