using SERP.Domain.Transactions.InboundShipments;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Request
{
    public class MappingAsnRequestDto
    {
        public int inboundShipmentId { get; set; }
        public HashSet<int> asnList { get; set; }
    }
}
