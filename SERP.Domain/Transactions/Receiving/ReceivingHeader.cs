using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common;
using SERP.Domain.Common.Constants;

namespace SERP.Domain.Transactions.Receiving
{
    public class ReceivingHeader : BaseModel
    {
        [StringLength(DomainDBLength.ReceivingNo)]
        public string receiving_no { get; set; }
        [StringLength(DomainDBLength.StatusFlag)]
        public string status_flag { get; set; }
        public int branch_plant_id { get; set; }
        public int? asn_header_id { get; set; }
        public int? supplier_id { get; set; }
        public DateTime? received_on { get; set; }
        public DateTime? inspector_assigned_on { get; set; }
        public DateTime? inspection_start_on { get; set; }
        public DateTime? inspection_end_on { get; set; }
        public DateTime? inspection_due_date { get; set; }
        [StringLength(DomainDBLength.UserId)]
        public string? inspected_by { get; set; }
        //[StringLength(DomainDBLength.UserId)]
        //public string? released_by { get; set; }
    }
}