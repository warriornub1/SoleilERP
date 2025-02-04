using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common.Model;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class SearchPagedAsnDetailRequestModel: SearchPagedRequestModel
    {
        [Required]
        public int asn_header_id { get; set; }
    }
}
