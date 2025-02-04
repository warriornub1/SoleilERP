using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Domain.Transactions.Containers.Model.Base
{
    public class ContainerRequestModel:BaseModel
    {
        public string container_no { get; set; }
        [AcceptValue(
            DomainConstant.Containers.StatusFlag.Incoming,
            DomainConstant.Containers.StatusFlag.Received,
            DomainConstant.Containers.StatusFlag.Unloaded,
            DomainConstant.Containers.StatusFlag.Completed,
            DomainConstant.Containers.StatusFlag.Unverified
        )]
        public int? inbound_shipment_blawb_id { get; set; }
        public string? bay_no { get; set; }
        public int? supplier_id { get; set; }
        public DateTime? detention_date { get; set; }
        public int? no_of_packages { get; set; }
        public int? no_of_packages_unloaded { get; set; }
        public string status_flag { get; set; }
        public string shipment_type { get; set; }
        public string container_type { get; set; }
        [DecimalPrecision(8, 2)]
        public decimal? weight { get; set; }
        public DateTime? unload_start_on { get; set; }
        public DateTime? unload_end_on { get; set; }
        public string unloaded_by { get; set; }
        public string unload_remark { get; set; }
        public DateTime? received_on { get; set; }
        public string received_by { get; set; }
        public DateTime? released_on { get; set; }
        public string released_by { get; set; }
    }
}
