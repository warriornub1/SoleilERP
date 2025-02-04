using SERP.Application.Common;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces
{
    public interface IASNDetailRepository: IGenericRepository<ASNDetail>
    {
        Task<List<AsnDetailResponseDetail>> GetAsnDetailDataAsync(ASNHeader asnHeader);
        HashSet<int> GetAsnHeaderContainItem(List<int> requestItems);
        IQueryable<PageAsnResponseDetail> BuildFilterAsnQuery(PagedFilterAsnRequestModel request, out int totalRows);
        IQueryable<PageAsnDetailResponseDetail> BuildAsnDetailQuery(PagedFilterAsnDetailRequestModel request);
        Task<List<ASNDetail>> GetASNDetailList(int asnHeaderId, List<int> poDetailIDs);
        Task<bool> IsAsnDetailWithStatusNotNew(int asnHeaderId);
        Task<List<ASNDetail>> GetAsnDetailByAsnHeaderIdAsync(int asnHeaderId);
    }
}
