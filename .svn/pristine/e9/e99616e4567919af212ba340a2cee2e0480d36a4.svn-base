using SERP.Application.Common;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.Invoice.Model;

namespace SERP.Application.Transactions.Invoices.Interfaces
{
    public interface IInvoiceDetailRepository: IGenericRepository<InvoiceDetail>
    {
        IQueryable<InvoiceDetailPagedResponseDetail> BuildInvoiceDetailPagedQuery(FilterInvoiceDetailPagedRequestModel filterInvoiceDetailPagedRequestModel, out int totalRows);
    }
}
