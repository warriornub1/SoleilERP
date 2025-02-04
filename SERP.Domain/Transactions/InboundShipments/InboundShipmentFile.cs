using SERP.Domain.Common;

namespace SERP.Domain.Transactions.InboundShipments
{
    public class InboundShipmentFile : BaseModel
    {
        public int inbound_shipment_id { get; set; }
        public int file_id { get; set; }
    }
}
