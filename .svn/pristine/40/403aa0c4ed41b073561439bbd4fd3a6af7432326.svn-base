using SERP.Application.Common;
using SERP.Domain.Transactions.PackingLists;
using SERP.Domain.Transactions.PackingLists.Model;
using System.Linq.Expressions;
using SERP.Domain.Transactions.Containers.Model;

namespace SERP.Application.Transactions.PackingLists.Interfaces
{
    public interface IPackingHeaderRepository : IGenericRepository<PackingHeader>
    {
        bool isPackingListExist(int containerId);
        Task<List<PackingListForReceiving>> GetPackingListForReceivingDetail(int containerId, int asnHeaderId);
        Task<List<PackingHeader>> GetPackingListHeaderByConditionAsync(Expression<Func<PackingHeader, bool>> predicate);
        IQueryable<PagedPackingInformationDetail> BuildPackingListQuery(FilterPackingListRequestModel filterPackingListRequestModel);
        Task<List<PackingListInformationDetail>> GetPackingHeaderAndContainerInfoAsync(Expression<Func<PackingHeader, bool>> predicate);
    }
}
