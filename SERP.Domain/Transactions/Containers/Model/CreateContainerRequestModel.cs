using SERP.Domain.Transactions.Containers.Model.Base;

namespace SERP.Domain.Transactions.Containers.Model
{
    public class CreateContainerRequestModel: ContainerRequestModel
    {
        public int? inbound_shipment_id { get; set; }
        public int? inbound_shipment_request_id { get; set; }
        public int? inbound_shipment_blawb_id { get; set; }
        
    }
}
