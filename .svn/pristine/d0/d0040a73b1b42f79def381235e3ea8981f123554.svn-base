using SERP.Application.Common;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.Invoice.Model;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.Invoices.Interfaces
{
    public interface IInvoiceHeaderRepository: IGenericRepository<InvoiceHeader>
    {
        IQueryable<InvoicePagedResponseDetail> BuildInvoicePagedQuery(FilterInvoicePagedRequestModel filterInvoicePagedRequestModel, out int totalRows);
        Task<InvoiceInfoResponseModel?> GetInvoiceAsync(Expression<Func<InvoiceHeader, bool>> condition);
        Task<bool> CheckInvoiceHeaderExistedAsync(int requestInvoiceHeaderId);
    }
}
