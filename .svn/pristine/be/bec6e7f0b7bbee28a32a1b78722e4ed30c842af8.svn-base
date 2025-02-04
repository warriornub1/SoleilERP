using SERP.Domain.Common;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.Containers
{
    public class ContainerASN : BaseModel
    {
        public int asn_header_id { get; set; }
        public int container_id { get; set; }
        [StringLength(DomainDBLength.PackingListNo)]
        public string packing_list_no { get; set; }
    }
}
