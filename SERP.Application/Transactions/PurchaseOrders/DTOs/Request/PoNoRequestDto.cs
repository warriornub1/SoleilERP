namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class PoNoRequestDto
    {
        public HashSet<string>? Status { get; set; }
        public HashSet<int>? Suppliers { get; set; }
        public string? branchPlantNo { get; set; }
        public bool onlyWithOpenQty { get; set; }
    }
}
