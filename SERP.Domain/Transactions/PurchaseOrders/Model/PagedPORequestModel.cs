using SERP.Domain.Common.Model;

namespace SERP.Domain.Transactions.PurchaseOrders.Model
{
    public class PagedPORequestModel : PagedRequestModel
    {
        public string companyNo { get; set; }
        public string bpNo { get; set; }
        public bool includeClosed { get; set; }
    }
}
