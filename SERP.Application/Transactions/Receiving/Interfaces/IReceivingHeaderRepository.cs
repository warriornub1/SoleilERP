using SERP.Application.Common;
using SERP.Domain.Common.Model;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.Receiving.Interfaces
{
    public interface IReceivingHeaderRepository : IGenericRepository<ReceivingHeader>
    {
        Task<PagedResponseModel<ReceivingInfoModel>> SearchReceivingAsync(List<Expression<Func<ReceivingInfoModel, bool>>> filters, string keyword, PagingUtilities pageable, int skipRow, List<Sortable> sortBy);
        Task<List<ReceivingHeader>> GetReceivingListAsync(Expression<Func<ReceivingHeader, bool>> filters);
    }
}
