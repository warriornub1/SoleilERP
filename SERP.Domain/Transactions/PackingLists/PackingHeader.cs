using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.PackingLists
{
    public class PackingHeader : BaseModel
    {
        [StringLength(20)]
        public string packing_list_no { get; set; }
        public int? asn_header_id { get; set; }
        public int container_id { get; set; }
    }
}
