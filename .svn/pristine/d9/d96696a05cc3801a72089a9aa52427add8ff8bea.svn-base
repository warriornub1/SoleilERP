using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERP.Domain.Common;

namespace SERP.Domain.Transactions.PurchaseOrders
{
    public class PODetail : BaseModel
    {
        /// <summary>
        /// PO Header ID	Foreign Key from POHeader
        /// </summary>
        public int po_header_id { get; set; }

        /// <summary>
        /// Line No
        /// </summary>
        public int line_no { get; set; }

        /// <summary>
        /// Line Type
        /// </summary>
        [StringLength(50)]
        public string line_type { get; set; }

        /// <summary>
        /// 01: Draft
        /// 02: New
        /// 11: In Process
        /// 30: Closed
        /// 90: Cancelled
        /// </summary>
        [StringLength(2)]
        public string status_flag { get; set; }

        /// <summary>
        /// Item ID	Foreign key from table Item.id
        /// </summary>
        public int item_id { get; set; }

        /// <summary>
        /// Item Description shown in PO
        /// </summary>
        public string po_item_description { get; set; }

        /// <summary>
        /// PO Qty
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// Open PO Qty
        /// </summary>
        public int open_qty { get; set; }

        /// <summary>
        /// PO UOM
        /// </summary>
        [StringLength(5)]
        public string? uom { get; set; }

        /// <summary>
        /// Supplier Item Mapping ID, Foreign key from table SupplierItemMapping.id
        /// </summary>
        public int supplier_item_mapping_id { get; set; }

        /// <summary>
        /// Ship To Branch Plant ID, Foreign key from table BranchPlant.id
        /// </summary>
        public int ship_to_branch_plant_id { get; set; }

        /// <summary>
        /// Unit Cost
        /// </summary>
        [Column(TypeName = "decimal(11,4)")]
        public decimal unit_cost { get; set; }

        /// <summary>
        /// Extended Cost
        /// (unit_cost - unit_discount)  * qty
        /// </summary>
        [Column(TypeName = "decimal(13,4)")]
        public decimal? extended_cost { get; set; }

        /// <summary>
        /// Discount by percentage
        /// </summary>
        [Column(TypeName = "decimal(11,4)")]
        public decimal? unit_discount { get; set; }

        /// <summary>
        /// Cost Rule
        /// </summary>
        public string cost_rule { get; set; }

        /// <summary>
        /// Secondary Supplier ID, Foreign key from table Supplier.id
        /// </summary>
        public int secondary_supplier_id { get; set; }

        /// <summary>
        /// Supplier Acknowledgement No
        /// </summary>
        [StringLength(255)]
        public string? supplier_acknowledgement_no { get; set; }

        /// <summary>
        /// Instruction To Supplier
        /// </summary>
        [StringLength(255)]
        public string? instruction_to_supplier { get; set; }

        /// <summary>
        /// Internal Reference
        /// </summary>
        [StringLength(255)]
        public string? internal_reference { get; set; }

        /// <summary>
        /// Notes To Warehouse
        /// </summary>
        [StringLength(255)]
        public string? notes_to_warehouse { get; set; }

        /// <summary>
        /// Requested Date
        /// </summary>
        public DateOnly? requested_date { get; set; }

        /// <summary>
        /// Quoted Earliest Ex Factory Date
        /// </summary>
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }

        /// <summary>
        /// Quoted Latest Ex Factory Date
        /// </summary>
        public DateOnly? quoted_ex_fac_date_latest { get; set; }

        /// <summary>
        /// Acknowledged Ex Warehouse Date
        /// </summary>
        public DateOnly? ack_ex_fac_date { get; set; }

        /// <summary>
        /// Forecast Ex Warehouse Date
        /// </summary>
        public DateOnly? forecast_ex_wh_date { get; set; }

        /// <summary>
        /// Collection Date
        /// </summary>
        public DateTime? collection_date_time { get; set; }
    }
}
