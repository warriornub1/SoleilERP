using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.Receiving.Interfaces;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.Receiving
{
    internal class ReceivingFileRepository : GenericRepository<ReceivingFile>, IReceivingFileRepository
    {
        public ReceivingFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ReceivingFileModel>> GetFileInfoAsync(int rcvHeaderId)
        {
            var query = from rcvFiles in _dbContext.ReceivingFiles.AsNoTracking()
                        join fileTracking in _dbContext.FilesTracking.AsNoTracking() on rcvFiles.file_id equals fileTracking.id
                        where rcvFiles.receiving_header_id == rcvHeaderId
                        select new ReceivingFileModel
                        {
                            id = rcvFiles.id,
                            receiving_detail_id = rcvFiles.receiving_detail_id,
                            file_name = fileTracking.file_name,
                            file_type = fileTracking.file_type,
                            url_path = fileTracking.url_path
                        };

            return await query.ToListAsync();
        }
    }
}
