using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.InboundShipments
{
    public class InboundShipmentBLAWB: BaseModel
    {
        public int inbound_shipment_id { get; set; }
        public int? asn_header_id { get; set; }
        [StringLength(50)]
        public string bl_awb_no { get; set; }
        public int? bl_awb_total_packages { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? bl_awb_total_gross_weight { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal? bl_awb_volume { get; set; }
        [StringLength(255)]
        public string bl_awb_cargo_description { get; set; }
    }
}
