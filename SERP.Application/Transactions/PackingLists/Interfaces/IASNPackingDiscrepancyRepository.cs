using SERP.Application.Common;
using SERP.Domain.Transactions.PackingLists;
using SERP.Domain.Transactions.PackingLists.Model;

namespace SERP.Application.Transactions.PackingLists.Interfaces
{
    public interface IASNPackingDiscrepancyRepository: IGenericRepository<ASNPackingDiscrepancy>
    {
        IQueryable<PackingDiscrepancyListResponseDetail> BuildPackingDiscrepancyListQuery(FilterPackingListRequestModel filterPackingListRequestModel);
        Task<List<ASNPackingDiscrepancy>> GetAsnPackingDiscrepancytByAsnHeaderIdsAsync(HashSet<int> asnHeaderIds);
    }
}
