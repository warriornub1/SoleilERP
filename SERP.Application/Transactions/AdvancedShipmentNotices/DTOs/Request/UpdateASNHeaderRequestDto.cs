using SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class UpdateASNHeaderRequestDto: ANSHeaderRequestDto
    {
        public int id { get; set; }
    }
}
