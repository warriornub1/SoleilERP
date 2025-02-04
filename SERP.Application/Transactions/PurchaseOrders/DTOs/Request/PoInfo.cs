using SERP.Domain.Transactions.FilesTracking;
using SERP.Domain.Transactions.PurchaseOrders;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class PoInfo
    {
        public POHeader POHeader { get; set; }
        public List<PODetail> PODetail { get; set; }
        public List<FileTracking> FileTrackings { get; set; }
    }
}
