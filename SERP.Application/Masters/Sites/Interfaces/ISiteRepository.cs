using SERP.Application.Common;
using SERP.Domain.Masters.Sites;

namespace SERP.Application.Masters.Sites.Interfaces
{
    public interface ISiteRepository : IGenericRepository<Site>
    {
        Task<List<Site>> GetAllLimited();
        Task<bool> FindSite(int site_id);
        Task<Dictionary<string, Site>> GetDictionaryBySiteNoAsync(HashSet<string?> siteNos);
        Task<bool> SiteExistsAsync(string siteNo);
        Task<List<Site>> GetSiteNoListAsync(HashSet<string> siteNo);
        Task<int[]> GetSiteAvailable(HashSet<int> siteIds);
        Task<Dictionary<string, int>> GetSiteIDDictionary(List<string?> siteIds);
        Task<IEnumerable<int>> GetIdWithFlag(List<int> Ids);
    }
}
