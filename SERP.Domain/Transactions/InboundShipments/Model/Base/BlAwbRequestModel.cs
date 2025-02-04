using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.InboundShipments.Model.Base
{
    public class BlAwbRequestModel
    {
        [StringLength(50)]
        public string bl_awb_no { get; set; }
        public int supplier_id { get; set; }
        public int? asn_header_id { get; set; }
        public int? bl_awb_total_packages { get; set; }
        [DecimalPrecision(8,2)]
        public decimal? bl_awb_total_gross_weight { get; set; }
        [DecimalPrecision(8,2)]
        public decimal? bl_awb_volume { get; set; }
        [StringLength(255)]
        public string? bl_awb_cargo_description { get; set; }
    }
}
