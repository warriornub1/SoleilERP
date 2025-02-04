using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.PackingLists.Interfaces;
using SERP.Domain.Transactions.PackingLists;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;

namespace SERP.Infrastructure.Transactions.PackingLists
{
    internal class PackingDetailRepository : GenericRepository<PackingDetail>, IPackingDetailRepository
    {
        public PackingDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PackingDetail>> GetPackingListDetailByConditionAsync(Expression<Func<PackingDetail, bool>> condition)
        {
            return await _dbContext.PackingDetails.Where(condition).ToListAsync();
        }
    }
}
