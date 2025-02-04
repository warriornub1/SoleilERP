using SERP.Domain.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Transactions.Invoices.DTOs.Request.Base
{
    public class InvoiceHeaderRequestDto
    {
        public string invoice_no { get; set; }
        public int supplier_id { get; set; }
        public int branch_plant_id { get; set; }
        [DecimalPrecision(9,2)]
        public decimal amt { get; set; }
        [DecimalPrecision(11,2)]
        public decimal total_amt { get; set; }
        [StringLength(1024)]
        public string? variance_reason { get; set; }
        public int currency_id { get; set; }
        public DateOnly? invoice_date { get; set; }
    }
}
