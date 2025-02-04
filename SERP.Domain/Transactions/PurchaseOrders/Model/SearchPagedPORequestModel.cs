using SERP.Domain.Common.Model;

namespace SERP.Domain.Transactions.PurchaseOrders.Model
{
    public class SearchPagedPORequestModel : SearchPagedRequestModel
    {
        public string companyNo { get; set; }
        public string bpNo { get; set; }
    }
}
