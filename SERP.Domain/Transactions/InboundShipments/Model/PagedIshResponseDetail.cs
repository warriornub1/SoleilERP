using SERP.Domain.Transactions.Containers.Model;

namespace SERP.Domain.Transactions.InboundShipments.Model
{
    public class PagedIshResponseDetail
    {
        public InboundShipmentDetail inbound_shipment { get; set; }
        public List<InboundShipmentPageAnsDetail> asns { get; set; }

        public List<InboundShipmentPageBlAwbDetail> bl_awbs { get; set; }
    }

    public class InboundShipmentDetail
    {
         public int id { get; set; }
        public string inbound_shipment_no { get; set; }
        public string freight_method { get; set; }
        public string incoterm { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
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
        public decimal? shipping_invoice_amt { get; set; }
        public string? shipping_invoice_currency { get; set; }
        public int? insurance_agent_id { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class InboundShipmentPageAnsDetail
    {
        public int id { get; set; }
        public string asn_no { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public bool shipment_arranged_supplier_flag { get; set; }
        public int? inbound_shipment_id { get; set; }
        public string ship_to_branch_plant_no { get; set; }
        public string ship_to_branch_plant_name { get; set; }
    }

    public class InboundShipmentPageBlAwbDetail
    {
        public string bl_awb_no { get; set; }
        public List<ContainerDetail>? containers { get; set; }
    }
}
