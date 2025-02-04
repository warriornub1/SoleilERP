using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.FilesTracking.Interfaces;
using SERP.Domain.Transactions.FilesTracking;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.FilesTracking
{
    internal class FileTrackingRepository : GenericRepository<FileTracking>, IFileTrackingRepository
    {
        public FileTrackingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsFileExists(string fileName)
        {
            return await _dbContext.FilesTracking.AnyAsync(x => x.file_name == fileName);
        }

        public async Task<string[]> ValidateFileNameInDatabase(string[] fileNames)
        {
            var existedFiles = await _dbContext.FilesTracking.Where(x =>
                    fileNames.Contains(x.file_name))
                .Select(x => x.file_name).Distinct()
                .ToArrayAsync();

            return existedFiles;
        }

        public async Task<List<FileTracking>> GetFileTrackingByIds(HashSet<int> listFileTrackingIDs)
        {
            return await _dbContext.FilesTracking.Where(x => listFileTrackingIDs.Contains(x.id)).ToListAsync();
        }
    }
}
