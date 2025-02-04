using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SERP.Domain.Common;

namespace SERP.Domain.Transactions.PurchaseOrders
{
    public class POHeader : BaseModel
    {
        [StringLength(15)]
        public string po_no { get; set; }

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
        /// STO: Standard Order
        /// DIS: Direct Shipment
        /// ITO: Intermediary Order
        /// DSO: Direct from Sales Order
        /// </summary>
        [StringLength(3)]
        public string po_type { get; set; }

        /// <summary>
        /// Date of PO
        /// </summary>
        public DateTime po_date { get; set; }

        /// <summary>
        /// Issuing Branch Plan id Foreign key from table BranchPlant.id
        /// </summary>
        public int branch_plant_id { get; set; }

        /// <summary>
        /// Supplier id Foreign key from table Supplier.id
        /// </summary>
        public int supplier_id { get; set; }

        /// <summary>
        /// Intermediary Supplier id Foreign key from table Supplier.id
        /// </summary>
        public int intermediary_supplier_id { get; set; }

        /// <summary>
        /// Secondary Supplier id Foreign key from table Supplier.id
        /// </summary>
        public int? secondary_supplier_id { get; set; }

        /// <summary>
        /// Ship To Branch Plant id Foreign key from table BranchPlant.id
        /// </summary>
        public int? ship_to_branch_plant_id { get; set; }

        /// <summary>
        /// Ship To Site id Foreign key from table Site.id
        /// </summary>
        public int? ship_to_site_id { get; set; }

        /// <summary>
        /// Forwarder id Foreign key from table Agent.id
        /// </summary>
        public int? forwarder_agent_id { get; set; }

        /// <summary>
        /// Sales Order id Foreign key from table SalesOrder.id
        /// </summary>
        public int? sales_order_id { get; set; }

        /// <summary>
        /// Payment Term
        /// </summary>
        [StringLength(50)]
        public string payment_term { get; set; }

        /// <summary>
        /// Incoterms
        /// </summary>
        [StringLength(50)]
        public string? incoterm { get; set; }

        /// <summary>
        /// Base Currency id Foreign key from table Currency.id
        /// </summary>
        public int base_currency_id { get; set; }

        /// <summary>
        /// Currency id Foreign key from table Currency.id
        /// </summary>
        public int po_currency_id { get; set; }

        /// <summary>
        /// exchange_rate, Exchange Rule
        /// </summary>
        [Column(TypeName = "decimal(13, 7)")]
        public decimal exchange_rate { get; set; }

        /// <summary>
        /// Cost Rule
        /// </summary>
        [StringLength(50)]
        public string? cost_rule { get; set; }

        /// <summary>
        /// Urgency Code
        /// </summary>
        [StringLength(50)]
        public string? urgency_code { get; set; }

        /// <summary>
        /// Order Discount, Discount by lump sum
        /// </summary>
        [Column(TypeName = "decimal(9, 2)")]
        public decimal? order_discount { get; set; }

        /// <summary>
        /// Taken By
        /// </summary>
        [StringLength(255)]
        public string? taken_by { get; set; }

        /// <summary>
        /// Internal Remarks
        /// </summary>
        [StringLength(1024)]
        public string? internal_remarks { get; set; }

        /// <summary>
        /// Freight Method
        /// </summary>
        [StringLength(50)]
        public string? freight_method { get; set; }

        /// <summary>
        /// Self Collect Site id Foreign key from table Site.id
        /// </summary>
        public int? self_collect_site_id { get; set; }

        /// <summary>
        /// Port Of Discharge id Foreign key from table Port.id
        /// </summary>
        public int? port_of_discharge_id { get; set; }

        /// <summary>
        /// Send Method
        /// </summary>
        [StringLength(50)]
        public string? send_method { get; set; }

        /// <summary>
        /// Quotation Reference
        /// </summary>
        [StringLength(255)]
        public string? quotation_reference { get; set; }

        /// <summary>
        /// Supplier Acknowledgement No
        /// </summary>
        [StringLength(255)]
        public string? supplier_acknowledgement_no { get; set; }

        /// <summary>
        /// Supplier Marking Reference
        /// </summary>
        [StringLength(255)]
        public string? supplier_marking_reference { get; set; }

        /// <summary>
        /// Notes To Supplier
        /// </summary>
        [StringLength(255)]
        public string? notes_to_supplier { get; set; }

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

        /// <summary>
        /// Delivery Date
        /// </summary>
        public DateTime? delivery_date { get; set; }

        /// <summary>
        /// Total Amount Base
        /// </summary>
        [Column(TypeName = "decimal(13,4)")]
        public decimal? total_amt_base { get; set; }

        /// <summary>
        /// Total Amount Foreign
        /// </summary>
        [Column(TypeName = "decimal(13,4)")]
        public decimal? total_amt_foreign { get; set; }

        /// <summary>
        /// Total Line Discount
        /// </summary>
        [Column(TypeName = "decimal(13,4)")]
        public decimal? total_line_discount { get; set; }

        /// <summary>
        /// Attachment Flag	To indicate if PO has attachment
        /// </summary>
        public bool attachment_flag { get; set; }

        /// <summary>
        /// Notes To Warehouse Flag	To indicate if any of the PO Line has Notes to Warehouse
        /// </summary>
        public bool notes_to_warehouse_flag { get; set; }
    }
}