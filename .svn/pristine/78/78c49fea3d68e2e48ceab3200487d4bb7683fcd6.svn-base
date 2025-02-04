using SERP.Application.Common;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.InboundShipments.Interfaces
{
    public interface IInboundShipmentRequestRepository: IGenericRepository<InboundShipmentRequest>
    {
        Task<List<string>> GetInboundShipmentRequestGroupList(string statusFlag);
        IQueryable<PagedIsrResponseDetail> BuildISRFilterQuery(PageFilterIsrRequestModel request);
        Task<List<InboundShipmentRequest>> GetByConditionAsync(Expression<Func<InboundShipmentRequest, bool>> func);
    }
}
