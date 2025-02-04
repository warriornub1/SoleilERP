using SERP.Domain.Common.Model;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class SearchPagedAsnRequestModel: SearchPagedRequestModel
    {
        [Required]
        public string companyNo { get; set; }
        [Required]
        public string bpNo { get; set; }
    }
}
