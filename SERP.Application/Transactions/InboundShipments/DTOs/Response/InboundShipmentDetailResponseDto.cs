using SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base;
using SERP.Application.Transactions.Containers.DTOs.Response;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Response
{
    public class InboundShipmentDetailResponseDto
    {
        public InboundShipmentResponse inbound_shipment { get; set; }
        public List<InboundShipmentDetailAnsResponse>? asns { get; set; }
        public List<InboundShipmentFileResponse>? inbound_shipment_files { get; set; }
        public List<InboundShipmentBlAwbResponse> bl_awb { get; set; }
    }

    public class InboundShipmentResponse
    {
        public int id { get; set; }
        public string inbound_shipment_no { get; set; }
        public string freight_method { get; set; }
        public string incoterm { get; set; }
        public DateOnly? cargo_ready_date { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
        public int branch_plant_id { get; set; }
        public string status_flag { get; set; }
        public int country_of_loading_id { get; set; }
        public string country_of_loading_name { get; set; }
        public int port_of_loading_id { get; set; }
        public string port_of_loading_name { get; set; }
        public int country_of_discharge_id { get; set; }
        public string country_of_discharge_name { get; set; }
        public int port_of_discharge_id { get; set; }
        public string port_of_discharge_name { get; set; }
        public string forwarder_no { get; set; }
        public string forwarder_name { get; set; }
        public string shipping_agent_no { get; set; }
        public string shipping_agent_name { get; set; }
        public string insurance_agent_no { get; set; }
        public string insurance_agent_name { get; set; }
        public string vessel_flight_no { get; set; }
        public string connecting_vessel_flight_no { get; set; }
        public DateOnly? notice_of_arrival_date { get; set; }
        public string import_permit_no { get; set; }
        public string internal_remarks { get; set; }
        public int? forwarder_agent_id { get; set; }
        public string forwarder_invoice_no { get; set; }
        public int? forwarder_invoice_currency_id { get; set; }
        public decimal? forwarder_invoice_amt { get; set; }
        public string? forwarder_invoice_currency { get; set; }
        public int? shipping_agent_id { get; set; }
        public string shipping_invoice_no { get; set; }
        public int? shipping_invoice_currency_id { get; set; }
        public decimal? shipping_invoice_amt { get; set; }
        public string? shipping_invoice_currency { get; set; }
        public int? insurance_agent_id { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class InboundShipmentDetailAnsResponse : AnsResponse
    {
        //public int? inbound_shipment_blawb_id { get; set; }
        public List<InvoiceAsnResponse>? invoice { get; set; }
        public List<ContainerResponseDto> containers { get; set; }

    }

    public class InboundShipmentFileResponse
    {
        public int id { get; set; }
        public string file_type { get; set; }
        public string document_type { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
    }

    public class InboundShipmentBlAwbResponse : InboundShipmentBlAwbResponseDto
    {
    }
}
