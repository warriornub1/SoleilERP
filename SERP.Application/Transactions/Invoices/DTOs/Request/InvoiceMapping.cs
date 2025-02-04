using SERP.Domain.Transactions.Invoice;

namespace SERP.Application.Transactions.Invoices.DTOs.Request
{
    public class InvoiceMapping
    {
        public InvoiceHeader InvoiceHeader { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
