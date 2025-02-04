using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.CustomViews.Interfaces;
using SERP.Domain.Transactions.CustomViews;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.CustomViews
{
    internal class CustomViewFilterRepository: GenericRepository<CustomViewFilter>, ICustomViewFilterRepository
    {
        public CustomViewFilterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<CustomViewFilter>> GetByCustomViewId(List<int> customViewIds)
        {
             return await _dbContext.CustomViewFilters
                 .Where(x => x.custom_view_id.HasValue)
                 .Where(x => customViewIds.Contains(x.custom_view_id!.Value)).ToListAsync();
        }
    }
}
