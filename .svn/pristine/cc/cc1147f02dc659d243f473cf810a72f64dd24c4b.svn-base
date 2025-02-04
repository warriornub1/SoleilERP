using SERP.Application.Common;
using SERP.Domain.Transactions.InboundShipments;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.InboundShipments.Interfaces
{
    public interface IInboundShipmentASNRepository: IGenericRepository<InboundShipmentASN>
    {
        Task<List<InboundShipmentASN>> GetByConditionAsync(Expression<Func<InboundShipmentASN, bool>> predicate);
    }
}
