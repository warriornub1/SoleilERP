using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common;
using SERP.Domain.Common.Constants;

namespace SERP.Domain.Transactions.PackingLists
{
    public class PackingDetail : BaseModel
    {
        public int packing_header_id { get; set; }
        public int asn_detail_id { get; set; }
        [StringLength(DomainDBLength.PackageNo)]
        public string? package_no { get; set; }
        [StringLength(DomainDBLength.MixedCartonNo)]
        public string? mixed_carton_no { get; set; }
        public int item_id { get; set; }
        public int qty { get; set; }
        public int? country_of_origin_id { get; set; }
        public int? no_of_carton { get; set; }
        public int? unit_per_carton { get; set; }
    }
}
