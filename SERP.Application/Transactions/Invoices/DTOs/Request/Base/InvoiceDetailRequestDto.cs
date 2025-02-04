using SERP.Domain.Common.Attributes;

namespace SERP.Application.Transactions.Invoices.DTOs.Request.Base
{
    public class InvoiceDetailRequestDto
    {
        public int po_detail_id { get; set; }
        public int qty { get; set; }
        [DecimalPrecision(11,4)]
        public decimal unit_price { get; set; }
        [DecimalPrecision(13,7)]
        public decimal exchange_rate { get; set; }
        public int? country_of_origin_id { get; set; }
    }
}
