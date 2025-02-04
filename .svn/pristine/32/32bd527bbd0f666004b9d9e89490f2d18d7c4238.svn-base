using SERP.Domain.Common;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.Invoice
{
    public class InvoiceHeader : BaseModel
    {
        public int branch_plant_id { get; set; }
        [StringLength(DomainDBLength.InvoiceHeader.InvoiceNo)]
        public string? invoice_no { get; set; }
        public int? asn_header_id { get; set; }
        public int? receiving_header_id { get; set; }
        public int currency_id { get; set; }
        public int supplier_id { get; set; }
        /// <summary>
        /// 01: Draft
        /// 02: New
        /// </summary>
        [StringLength(DomainDBLength.InvoiceHeader.StatusFlag)]
        public string status_flag { get; set; }
        [Column(TypeName = "decimal(11,4)")]
        public decimal amt { get; set; }
        [Column(TypeName = "decimal(13,4)")]
        public decimal total_amt { get; set; }
        [StringLength(DomainDBLength.InvoiceHeader.VarianceReason)]
        public string? variance_reason { get; set; }
        /// <summary>
        /// Date of Invoice
        /// </summary>
        public DateOnly? invoice_date { get; set; }
    }
}
