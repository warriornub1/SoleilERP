using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.Invoice
{
    public class InvoiceDetail : BaseModel
    {
        public int invoice_header_id { get; set; }

        public int line_no { get; set; }

        public int po_detail_id { get; set; }

        public int qty { get; set; }

        [Column(TypeName = "decimal(13,7)")]
        public decimal? exchange_rate { get; set; }

        [Column(TypeName = "decimal(11,4)")]
        public decimal unit_price { get; set; }

        [Column(TypeName = "decimal(13,4)")]
        public decimal total_price { get; set; }

        public int? country_of_origin_id { get; set; }
    }
}
