using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.Containers.Interfaces;
using SERP.Domain.Transactions.Containers;
using SERP.Domain.Transactions.Containers.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.Containers
{
    internal class ContainerFileRepository : GenericRepository<ContainerFile>, IContainerFileRepository
    {
        public ContainerFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<ContainerFileModel>> GetFileInfoAsync(int containerId)
        {
            var query = from containerFile in _dbContext.ContainerFiles.AsNoTracking()
                        join fileTracking in _dbContext.FilesTracking.AsNoTracking() on containerFile.file_id equals fileTracking.id
                        where containerFile.container_id == containerId
                        select new ContainerFileModel
                        {
                            file_id = containerFile.id,
                            container_file_type = containerFile.container_file_type,
                            url_path = fileTracking.url_path,
                            thumbnail_url_path = fileTracking.url_path_thumbnail
                        };

            return await query.ToListAsync();
        }
    }
}
