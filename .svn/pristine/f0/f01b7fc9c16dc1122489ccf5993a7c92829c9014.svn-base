using SERP.Application.Common;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Domain.Transactions.PurchaseOrders.Model;

namespace SERP.Application.Transactions.PurchaseOrders.Interfaces
{
    public interface IPODetailRepository : IGenericRepository<PODetail>
    {
        Task<Dictionary<int, List<int>>> GetSupplierIdsAsync(HashSet<int> poDetailIds);
        HashSet<int> GetPoHeaderContainItem(List<int> requestItemIDs);
        Task<int[]> GetPoHeaderWithOpenQtyAsync(HashSet<string>? poHeaderStatus);
        Task<int[]> GetPoDetailAvailable(HashSet<int> poDetailIDs, List<string> unexpectedStatus);
        Task<List<PODetail>> GetPoDetailList(List<int> poDetailIDs);
        Task<Dictionary<int, AsnPoResponseDetail>> GetDictionaryPoDataAsync(HashSet<int> poDetailIds);
        IQueryable<PoResponseDetail> BuildPoFilterQuery(PagedFilterPoRequestModel request, out int totalRows);
        List<PODetail> GetPoDetailByHeaderIdAsync(int poHeaderId);
        IQueryable<PagePoDetailResponseDetail> BuildPoDetailFilterQuery(PagedFilterPoRequestModel request, out int totalRows);
        Task<List<PODetail>> GetPoLineForDeleteAsync(int requestPoHeaderId, List<int> requestPoDetailIDs);
        Task<List<PoNoListResponseDetail>> GetPoNoInfoList(PoNoRequestDetail request);
        Task<bool> IsPoDetailWithStatusNotNew(int poHeaderId);
    }
}
