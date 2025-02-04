using SERP.Domain.Transactions.Containers.Model.Base;

namespace SERP.Domain.Transactions.Containers.Model
{
    public class ContainerInfoModel : ContainerRequestModel
    {
        public ContainerAsns[] asns { get; set; } = [];
        public ContainerInboundShipment? inbound_shipment { get; set; }
        public ContainerInboundShipmentBlawb? inbound_shipment_blawb { get; set; }
        public ContainerReceivings[] receivings { get; set; } = [];
        public ContainerPackingList[] packingLists { get; set; } = [];
    }

    public class ContainerAsns
    {
        public int asn_header_id { get; set; }
        public string asn_no { get; set; }
        public string notes_to_cargo_team { get; set; }
    }
    public class ContainerReceivings
    {
        public int receiving_header_id { get; set; }
        public string receiving_no { get; set; }
    }
    public class ContainerPackingList
    {
        public int packing_header_id { get; set; }
        public int? asn_header_id { get; set; }
        public string packing_list_no { get; set; }
    }
    public class ContainerInboundShipment
    {
        public string inbound_shipment_no { get; set; }
        public DateOnly? notice_of_arrival_date { get; set; }
    }
    public class ContainerInboundShipmentBlawb
    {
        public string bl_awb_no { get; set; }
        public int supplier_id { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public int? bl_awb_total_package { get; set; }
        public decimal? bl_awb_total_gross_weight { get; set; }
        public decimal? bl_awb_volumn {  get; set; }
        public string bl_awb_cargo_description { get; set; }
    }
}
