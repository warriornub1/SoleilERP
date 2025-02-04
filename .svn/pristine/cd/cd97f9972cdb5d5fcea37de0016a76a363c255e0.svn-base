using SERP.Domain.Common;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.InboundShipments
{
    public class InboundShipmentRequest : BaseModel
    {
        public int asn_header_id { get; set; }
        /// <summary>
        /// ISRYYMM99999
        /// </summary>
        [StringLength(DomainDBLength.InboundShipmentRequest.InboundShipmentRequestNo)]
        public string inbound_shipment_request_no { get; set; }
        /// <summary>
        /// ISGYYMM99999
        /// </summary>
        [StringLength(DomainDBLength.InboundShipmentRequest.InboundShipmentRequestGroupNo)]
        public string? inbound_shipment_request_group_no { get; set; }
        /// <summary>
        /// Status Flag
        /// 02: New
        /// 10: Arranged
        /// 90: Cancelled
        /// </summary>
        [StringLength(DomainDBLength.InboundShipmentRequest.StatusFlag)]
        public string status_flag { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.FreightMethod)]
        public string freight_method { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.Incoterm)]
        public string incoterm { get; set; }
        public DateOnly? cargo_ready_date { get; set; }
        public DateOnly? port_of_loading_etd { get; set; }
        public DateOnly? port_of_discharge_eta { get; set; }
        public int? country_of_loading_id { get; set; }
        public int? port_of_loading_id { get; set; }
        public int? country_of_discharge_id { get; set; }
        public int? port_of_discharge_id { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.CargoDescription)]
        public string cargo_description { get; set; }
        /// <summary>
        /// L: Loose F: Full
        /// </summary>
        [StringLength(DomainDBLength.InboundShipmentRequest.ShipmentType)]
        public string? shipment_type { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.HSCode)]
        public string? hs_code { get; set; }
        public int? total_packages { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? total_gross_weight { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? volume { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.MarkingCargo)]
        public string? marking_cargo { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.MarkingBL)]
        public string? marking_bl { get; set; }
        [StringLength(DomainDBLength.InboundShipmentRequest.CollectionAddress)]
        public string? collection_address { get; set; }
    }
}
