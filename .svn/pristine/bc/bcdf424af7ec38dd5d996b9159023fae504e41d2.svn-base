using SERP.Application.Transactions.Invoices.DTOs.Request.Base;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.Invoices.DTOs.Request
{
    public class CreateInvoiceRequestDto
    {
        [AcceptValue(DomainConstant.Action.Submit,
            DomainConstant.Action.Draft)]
        public string action { get; set; }
        public List<CreateInvoiceInfo> invoices { get; set; }
    }

    public class CreateInvoiceInfo
    {
        public InvoiceHeaderRequestDto? invoice_header { get; set; }
        public List<InvoiceDetailRequestDto>? invoice_details { get; set; }
    }
}
