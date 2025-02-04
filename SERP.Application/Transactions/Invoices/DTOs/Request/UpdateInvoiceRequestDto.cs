using SERP.Application.Transactions.Invoices.DTOs.Request.Base;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.Invoices.DTOs.Request
{
    public class UpdateInvoiceRequestDto
    {
        [AcceptValue(DomainConstant.Action.Submit,
            DomainConstant.Action.Draft)]
        public string action { get; set; }
        public List<UpdateInvoiceInfo> invoices { get; set; }
        public List<int>? delete_detail_id { get; set; }
    }

    public class UpdateInvoiceInfo
    {
        public UpdateInvoiceHeaderRequestDto? invoice_header { get; set; }
        public List<UpdateInvoiceDetailRequestDto>? invoice_details { get; set; }
    }

    public class UpdateInvoiceHeaderRequestDto : InvoiceHeaderRequestDto
    {
        public int id { get; set; }
    }

    public class UpdateInvoiceDetailRequestDto : InvoiceDetailRequestDto
    {
        public int id { get; set; }
    }

}
