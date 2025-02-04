using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.Sites.Interfaces;
using SERP.Domain.Masters.Sites;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Sites
{
    internal class SiteRepository : GenericRepository<Site>, ISiteRepository
    {
        public SiteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Site>> GetAllLimited()
        {
            return await _dbContext.Site.Select(x => new Site
            {
                id = x.id,
                site_no = x.site_no,
                site_name = x.site_name
            }).ToListAsync();
        }

        public async Task<bool> FindSite(int site_id)
        {
            var currency = await _dbContext.Site.Where(x => x.id == site_id)
                                                .FirstOrDefaultAsync();

            return currency == null;

        }

        public async Task<Dictionary<string, Site>> GetDictionaryBySiteNoAsync(HashSet<string?> siteNos)
        {
            return await _dbContext.Site
                .Where(x => siteNos.Contains(x.site_no))
                .ToDictionaryAsync(x => x.site_no);
        }

        public async Task<bool> SiteExistsAsync(string siteNo)
        {
            return await _dbContext.Site.AnyAsync(x => x.site_no.Equals(siteNo));
        }

        public async Task<List<Site>> GetSiteNoListAsync(HashSet<string> siteNo)
        {
            return await _dbContext.Site
                .Where(x => siteNo.Contains(x.site_no))
                .Select(x => new Site
                {
                    id = x.id,
                    site_no = x.site_no
                }).ToListAsync();
        }

        public async Task<int[]> GetSiteAvailable(HashSet<int> siteIds)
        {
            return await _dbContext.Site
                .Where(x => siteIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }

        public async Task<Dictionary<string, int>> GetSiteIDDictionary(List<string?> siteIds)
        {
            return await _dbContext.Site.Where(x => siteIds.Contains(x.site_no))
                                        .Select(x => new { x.id, x.site_no })
                                        .ToDictionaryAsync(x => x.site_no, x=> x.id);
        }

        public async Task<IEnumerable<int>> GetIdWithFlag(List<int> Ids)
        {
            return await _dbContext.Site.Where(x => Ids.Contains(x.id))
                                            .Select(x => x.id)
                                            .ToListAsync();
        }
    }
}
