using SERP.Application.Transactions.Invoices.DTOs.Request.Base;

namespace SERP.Application.Transactions.Invoices.DTOs.Request
{
    public class ValidateInvoiceRequest
    {
        public InvoiceHeaderRequestDto invoiceHeader { get; set; }
        public List<InvoiceDetailRequestDto> invoiceDetails { get; set; }
    }
}
