using SERP.Application.Common;
using SERP.Domain.Transactions.Containers;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.Containers.Interfaces
{
    public interface IContainerAsnRepository: IGenericRepository<ContainerASN>
    {
        Task<List<ContainerASN>> GetContainerAsnByConditionAsync(Expression<Func<ContainerASN, bool>> predicate);
        Task<bool> ValidateShipmentTypeInAsnAsync(int containerId, int asnHeaderId, string requestShipmentType);
    }
}
