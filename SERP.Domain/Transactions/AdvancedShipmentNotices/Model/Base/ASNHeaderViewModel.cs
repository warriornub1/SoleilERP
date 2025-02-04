using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model.Base
{
    public class ASNHeaderViewModel
    {
        public string status_flag { get; set; }
        public int branch_plant_id { get; set; }
        public int ship_to_branch_plant_id { get; set; }
        public int supplier_id { get; set; }
        public int? inbound_shipment_id { get; set; }
        public int? inbound_shipment_request_id { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        [StringLength(20)]
        public string? invoice_no { get; set; }
        [DecimalPrecision(9, 2)]
        public decimal? invoice_amt { get; set; }
        [DecimalPrecision(11, 2)]
        public decimal? total_asn_amt { get; set; }
        public int invoice_currency_id { get; set; }
        [DecimalPrecision(13, 7)]
        public decimal? invoice_exchange_rate { get; set; }
        [StringLength(1024)]
        public string? internal_remarks { get; set; }
        [StringLength(255)]
        public string? notes_to_cargo_team { get; set; }
        public int? total_packages { get; set; }
        [DecimalPrecision(8, 2)]
        public decimal? total_gross_weight { get; set; }
        [DecimalPrecision(8, 2)]
        public decimal? volume { get; set; }
        public bool shipment_arranged_supplier_flag { get; set; }
        public string? variance_reason { get; set; }
        public bool new_shipment_arranged_supplier_flag { get; set; }
        public string? inbound_shipment_request_group_no { get; set; }
    }
}
