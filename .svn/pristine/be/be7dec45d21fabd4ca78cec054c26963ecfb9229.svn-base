using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Request
{
    public class CreatePoDetailRequestDto
    {
        public string status_flag { get; set; }
        public int line_no { get; set; }
        public string line_type { get; set; }
        public int? item_id { get; set; }
        public string po_item_description { get; set; }
        public int? qty { get; set; }
        public int? open_qty { get; set; }
        public string? uom { get; set; }
        public int? supplier_item_mapping_id { get; set; }
        public int? ship_to_branch_plant_id { get; set; }
        [DecimalPrecision(11,4)]
        public decimal unit_cost { get; set; }
        public string cost_rule { get; set; }
        [DecimalPrecision(11,4)]
        public decimal? unit_discount { get; set; }
        public int? secondary_supplier_id { get; set; }
        [StringLength(255)]
        public string? supplier_acknowledgement_no { get; set; }
        [StringLength(255)]
        public string? instruction_to_supplier { get; set; }
        [StringLength(255)]
        public string? internal_reference { get; set; }
        [StringLength(255)]
        public string? notes_to_warehouse { get; set; }
        public DateOnly? requested_date { get; set; }
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }
        public DateOnly? quoted_ex_fac_date_latest { get; set; }
        public DateOnly? ack_ex_fac_date { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        public DateTime? collection_date_time { get; set; }
    }
}
