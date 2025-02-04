using SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Base;

namespace SERP.Application.Transactions.InboundShipments.DTOs.Response
{
    public class PagedIsrResponseDto
    {
        public InboundShipmentRequestPageResponse inbound_shipment_request {get; set; }
        public InboundShipmentRequestPageAsnResponse asn { get; set; }
    }

    public class InboundShipmentRequestPageAsnResponse : AnsResponse
    {
        public int id { get; set; }
        public string asn_no { get; set; }
        public string ship_to_branch_plant_no { get; set; }
        public string ship_to_branch_plant_name { get; set; }
        public string status_flag { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }

        public List<InvoiceAsnResponse>? invoice { get; set; }
        //// for update asn
        //public int branch_plant_id { get; set; }
        //public int supplier_id { get; set; }
        //public int? inbound_shipment_id { get; set; }
        //public int? inbound_shipment_request_id { get; set; }
        //public DateOnly? forecast_ex_wh_date { get; set; }
        //public int invoice_currency_id { get; set; }
        //public decimal? invoice_exchange_rate { get; set; }
        //public string? internal_remarks { get; set; }
        //public string? notes_to_cargo_team { get; set; }
        //public bool shipment_arranged_supplier_flag { get; set; }
    }

    public class InboundShipmentRequestPageResponse
    {
        public int inbound_shipment_request_id { get; set; }
        public string inbound_shipment_request_no { get; set; }
        public string inbound_shipment_request_group_no { get; set; }
        public int branch_plant_id { get; set; }
        public string status_flag { get; set; }
        public string freight_method { get; set; }
        public string incoterm { get; set; }
        public DateOnly? cargo_ready_date { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
        public string cargo_description { get; set; }
        public int? country_of_loading_id { get; set; }
        public string country_of_loading_name { get; set; }
        public int? port_of_loading_id { get; set; }
        public string? port_of_loading_no { get; set; }
        public string? port_of_loading_name { get; set; }
        public int? country_of_discharge_id { get; set; }
        public string country_of_discharge_name { get; set; }
        public int? port_of_discharge_id { get; set; }
        public string port_of_discharge_no { get; set; }
        public string port_of_discharge_name { get; set; }
        public string shipment_type { get; set; }
        public string hs_code { get; set; }
        public int total_packages { get; set; }
        public decimal total_gross_weight { get; set; }
        public decimal volume { get; set; }
        public string marking_cargo { get; set; }
        public string marking_bl { get; set; }
        public string collection_address { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class InvoiceAsnResponse
    {
        public string invoice_no { get; set; }
        public string invoice_currency { get; set; }
        public decimal invoice_amt { get; set; }
    }
}
