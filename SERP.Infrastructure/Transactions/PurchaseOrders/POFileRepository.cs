using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Domain.Transactions.PurchaseOrders.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.PurchaseOrders
{
    internal class POFileRepository : GenericRepository<POFile>, IPOFileRepository
    {
        public POFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PoFileResponseDetail>> GetFileInfoAsync(int poHeaderId)
        {
            var query = from poFile in _dbContext.PoFiles.AsNoTracking()
                        join fileTracking in _dbContext.FilesTracking.AsNoTracking() on poFile.file_id equals fileTracking.id
                        where poFile.po_header_id == poHeaderId
                        select new PoFileResponseDetail
                        {
                            id = poFile.id,
                            file_name = fileTracking.file_name,
                            url_path = fileTracking.url_path,
                            file_type = fileTracking.file_type,
                            created_by = poFile.created_by,
                            created_on = poFile.created_on
                        };

            return await query.ToListAsync();
        }

        public async Task<Dictionary<int, bool>> HasAttachmentCheck(HashSet<int> poHeaderIDs)
        {
            var result = await _dbContext.PoFiles.AsNoTracking()
                .Where(x => poHeaderIDs.Contains(x.po_header_id))
                .GroupBy(x => x.po_header_id)
                .ToDictionaryAsync(x => x.Key, x => true);
            return result;
        }
    }
}
