using SERP.Application.Common;
using SERP.Domain.Transactions.Containers;
using SERP.Domain.Transactions.Containers.Model;

namespace SERP.Application.Transactions.Containers.Interfaces
{
    public interface IContainerFileRepository: IGenericRepository<ContainerFile>
    {
        Task<List<ContainerFileModel>> GetFileInfoAsync(int containerId);
    }
}
