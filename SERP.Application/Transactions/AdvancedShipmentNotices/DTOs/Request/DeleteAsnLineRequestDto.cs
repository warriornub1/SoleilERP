namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class DeleteAsnLineRequestDto
    {
        public int AsnHeaderId { get; set; }
        public List<int> AsnDetailIDs { get; set; }
    }
}
