using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERP.Domain.Common.Attributes;
using static SERP.Domain.Common.Constants.DomainConstant;


namespace SERP.Domain.Transactions.PurchaseOrders.Model.Base
{
    public class POHeaderViewModel
    {
        [Required]
        public string status_flag { get; set; }
        [Required]
        [AcceptValue(
            Common.Constants.DomainConstant.PurchaseOrder.Type.StandardOrder,
            Common.Constants.DomainConstant.PurchaseOrder.Type.DirectFromSalesOrder,
            Common.Constants.DomainConstant.PurchaseOrder.Type.DirectShipment,
            Common.Constants.DomainConstant.PurchaseOrder.Type.IntermediaryOrder)]
        public string po_type { get; set; }
        [Required]
        public DateTime po_date { get; set; }
        public int? company_id { get; set; }
        [Required]
        public int issuing_branch_plant_id { get; set; }
        [Required]
        public int supplier_id { get; set; }
        public int? intermediary_supplier_id { get; set; }
        public int? secondary_supplier_id { get; set; }
        public int? ship_to_branch_plant_id { get; set; }
        public int? ship_to_site_id { get; set; }
        public int? forwarder_agent_id { get; set; }
        public int? sales_order_id { get; set; }
        [Required]
        [StringLength(50)]
        public string payment_term { get; set; }

        public string? incoterm { get; set; }
        [Required]
        public int base_currency_id { get; set; }
        [Required]
        public int po_currency_id { get; set; }
        [Required]
        [DecimalPrecision(13, 7)]
        public decimal exchange_rate { get; set; }
        public string? cost_rule { get; set; }
        [Required]
        public string urgency_code { get; set; }
        public decimal? order_discount { get; set; }
        [StringLength(255)]
        public string? taken_by { get; set; }
        [StringLength(1024)]
        public string? internal_remarks { get; set; }
        public string? freight_method { get; set; }
        public int? self_collect_site_id { get; set; }
        public int? port_of_discharge_id { get; set; }
        public string? send_method { get; set; }
        [StringLength(255)]
        public string? quotation_reference { get; set; }
        [StringLength(255)]
        public string? supplier_acknowledgement_no { get; set; }
        [StringLength(255)]
        public string? supplier_marking_reference { get; set; }
        public string? notes_to_supplier { get; set; }
        public DateOnly? requested_date { get; set; }
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }
        public DateOnly? quoted_ex_fac_date_latest { get; set; }
        public DateOnly? ack_ex_fac_date { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        public DateTime? collection_date_time { get; set; }
        public decimal? total_amt_base { get; set; }
        public decimal? total_amt_foreign { get; set; }
    }
}
