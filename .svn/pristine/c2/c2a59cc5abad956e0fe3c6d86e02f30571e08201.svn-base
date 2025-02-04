using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.SystemKVSs.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.SystemKVSs;
using SERP.Domain.Masters.SystemKVSs.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.SystemKVSs
{
    public class SystemKvsRepository : GenericRepository<SystemKvs>, ISystemKvsRepository
    {
        public SystemKvsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<SystemKvs> GetSystemKvsByKeywordAsync(string keyword)
        {
            return await _dbContext.SystemKvs.Where(x=>x.keyword==keyword).FirstOrDefaultAsync();
        }

        public async Task<FileUploadLimitedDetail?> GetFileUploadLimited()
        {
            var fileLimited = await _dbContext.SystemKvs.Where(x =>
                x.keyword.Equals(DomainConstant.SystemKvsKeyword.AllowedFileExt) ||
                x.keyword.Equals(DomainConstant.SystemKvsKeyword.FileSizeLimit)).ToListAsync();

            if (fileLimited.Count == 0)
            {
                return null;
            }

            var fileLimitedDetail = new FileUploadLimitedDetail
            {
                AllowedFileExtension = fileLimited.Find(x => x.keyword.Equals(DomainConstant.SystemKvsKeyword.AllowedFileExt))?.varchar_value,
                FileSizeLimit = fileLimited.Find(x => x.keyword.Equals(DomainConstant.SystemKvsKeyword.FileSizeLimit))?.number_value
            };

            return fileLimitedDetail;
        }
    }
}
