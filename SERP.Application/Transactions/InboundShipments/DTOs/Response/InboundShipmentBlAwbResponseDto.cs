namespace SERP.Application.Transactions.InboundShipments.DTOs.Response
{
    public class InboundShipmentBlAwbResponseDto
    {
        public int inbound_shipment_blawb_id { get; set; }
        public int? asn_header_id { get; set; }
        public string bl_awb_no { get; set; }
        public int? bl_awb_total_packages { get; set; }
        public decimal? bl_awb_total_gross_weight { get; set; }
        public decimal? bl_awb_volume { get; set; }
        public string bl_awb_cargo_description { get; set; }
    }
}
