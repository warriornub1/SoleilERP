using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices
{
    public class ASNDetail : BaseModel
    {
        public int asn_header_id { get; set; }
        /// <summary>
        /// Line No
        /// </summary>
        public int line_no { get; set; }
        /// <summary>
        /// FK from table PODetail
        /// </summary>
        public int po_detail_id { get; set; }
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
        /// Item ID	FK from table Item
        /// </summary>
        public int item_id { get; set; }
        /// <summary>
        /// ASN Qty
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// Packing List Qty
        /// </summary>
        public int packing_list_qty { get; set; }
        /// <summary>
        /// ASN UOM from Invoice
        /// </summary>
        [StringLength(5)]
        public string? uom { get; set; }
        /// <summary>
        /// Country Of Origin ID	FK from table Country
        /// </summary>
        public int? country_of_origin_id { get; set; }
        /// <summary>
        /// Notes To Warehouse
        /// </summary>
        [StringLength(255)]
        public string? notes_to_warehouse { get; set; }
    }
}
