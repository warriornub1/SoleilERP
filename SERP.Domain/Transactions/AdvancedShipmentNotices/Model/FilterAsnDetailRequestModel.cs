using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class FilterAsnDetailRequestModel
    {
        public HashSet<int>? poHeaderIDs { get; set; }
        public HashSet<string> statuses { get; set; }
    }
}
