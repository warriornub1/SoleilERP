using SERP.Application.Common;
using SERP.Domain.Transactions.PackingLists;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.PackingLists.Interfaces
{
    public interface IPackingDetailRepository : IGenericRepository<PackingDetail>
    {
         Task<List<PackingDetail>> GetPackingListDetailByConditionAsync(Expression<Func<PackingDetail, bool>> condition);
    }
}
