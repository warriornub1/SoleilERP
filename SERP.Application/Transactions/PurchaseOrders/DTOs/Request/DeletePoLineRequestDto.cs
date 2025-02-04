namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class DeletePoLineRequestDto
    {
        public int PoHeaderId { get; set; }
        public List<int> PoDetailIDs { get; set; }
    }
}
