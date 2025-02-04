using SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base;
using SERP.Application.Transactions.Containers.DTOs.Response;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Response
{
    public class PagedIshResponseDto
    {
        public InboundShipmentResponse inbound_shipment { get; set; }
        public List<InboundShipmentPageAnsResponse> asns { get; set; }
        public List<InboundShipmentPageBlAwbResponse> bl_awbs { get; set; }
    }

    public class InboundShipmentPageAnsResponse : AnsResponse
    {
        public bool shipment_arranged_supplier_flag { get; set; }
        public int? inbound_shipment_id { get; set; }
        public string ship_to_branch_plant_no { get; set; }
        public string ship_to_branch_plant_name { get; set; }
    }

    public class InboundShipmentPageBlAwbResponse
    {
        public string bl_awb_no { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public List<ContainerResponseDto>? containers { get; set; }
    }
}
