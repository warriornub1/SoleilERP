using SERP.Application.Common;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Domain.Transactions.PurchaseOrders.Model;

namespace SERP.Application.Transactions.PurchaseOrders.Interfaces
{
    public interface IPOFileRepository : IGenericRepository<POFile>
    {
        Task<List<PoFileResponseDetail>> GetFileInfoAsync(int poHeaderId);
        Task<Dictionary<int, bool>> HasAttachmentCheck(HashSet<int> poHeaderIDs);
    }
}
