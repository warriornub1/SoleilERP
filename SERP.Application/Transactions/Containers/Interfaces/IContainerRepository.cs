using SERP.Application.Common;
using SERP.Domain.Common.Model;
using SERP.Domain.Transactions.Containers;
using SERP.Domain.Transactions.Containers.Model;
using System.Linq.Expressions;

namespace SERP.Application.Transactions.Containers.Interfaces
{
    public interface IContainerRepository : IGenericRepository<Container>
    {
        Task<Dictionary<int, List<ContainerDetail>>> GetContainerByAsnHeaderIds(List<int> asnHeaderIds);
        Task<PagedResponseModel<ContainerDetailModel>> SearchContainerAsync(List<Expression<Func<ContainerDetailModel, bool>>> filters, PagingUtilities pageable, int skipRow, List<Sortable> sortBy);
        Task<ContainerInfoModel?> GetContainerByIdAsync(int id);
        Task<List<Container>> GetContainerListAsync(string bpNo, Expression<Func<Container, bool>> filters);
        Task<Dictionary<string, int>> GetContainerIdByContainerNoAsync(HashSet<string> containerNOs);
        Task<List<string>> GetContainerNoCompletedAsync(List<string> containerNosInRequest);
        Task<List<int>> GetExistingContainersWithAvailableByIdAsync(List<int> containerIdsInRequest);
        Task<List<ContainerListForAsnResponseDetail>> GetContainerListForAsnAsync(int asnHeaderId);
        Task<List<Container>> GetContainerByIdsAsync(List<int> containerIds);
        Task<List<Container>> GetContainerByConditionAsync(Expression<Func<Container, bool>> predicated);
    }
}
