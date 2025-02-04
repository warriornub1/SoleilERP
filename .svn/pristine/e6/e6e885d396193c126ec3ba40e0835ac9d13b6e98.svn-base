using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.CustomViews.Interfaces;
using SERP.Domain.Transactions.CustomViews;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.CustomViewAttributes
{
    public class CustomViewAttributeRepository : GenericRepository<CustomViewAttribute>, ICustomViewAttributeRepository
    {
        public CustomViewAttributeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<CustomViewAttribute>> GetByCustomViewId(List<int> customViewIds)
        {
            return await _dbContext.CustomViewAttributes.Where(x => customViewIds.Contains(x.custom_view_id)).ToListAsync();
        }
    }
}
