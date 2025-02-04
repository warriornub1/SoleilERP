using SERP.Application.Transactions.Invoices.Interfaces;
using SERP.Domain.Transactions.Invoice;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.Invoices
{
    internal class InvoiceFileRepository: GenericRepository<InvoiceFile>, IInvoiceFileRepository
    {
        public InvoiceFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
