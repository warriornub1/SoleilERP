using SERP.Domain.Transactions.PurchaseOrders.Model.Base;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class ValidatePORequest
    {
        public POHeaderViewModel po_header { get; set; }
        public List<PODetailViewModel> po_detail { get; set; }
    }
}
