using System.ComponentModel.DataAnnotations;
using SERP.Domain.Common;
using SERP.Domain.Common.Constants;

namespace SERP.Domain.Transactions.Receiving
{
    public class ReceivingDetail : BaseModel
    {
        public int receiving_header_id { get; set; }
        public int line_no { get; set; }
        public int? po_detail_id { get; set; }
        public int? asn_detail_id { get; set; }
        [StringLength(DomainDBLength.StatusFlag)]
        public string status_flag { get; set; }
        public int item_id { get; set; }
        public int qty { get; set; }
        public int inspected_qty { get; set; }
        [StringLength(DomainDBLength.Uom)]
        public string uom { get; set; }
        [StringLength(DomainDBLength.Uom)]
        public string? po_uom { get; set; }
        public int? packing_header_id { get; set; }
        [StringLength(DomainDBLength.PackageNo)]
        public string? package_no { get; set; }
        public int? country_of_origin_id { get; set; }
        [StringLength(DomainDBLength.SupplierDocType)]
        public string? supplier_document_type { get; set; }
        [StringLength(DomainDBLength.SupplierDocNo)]
        public string? supplier_document_no { get; set; }
    }
}
