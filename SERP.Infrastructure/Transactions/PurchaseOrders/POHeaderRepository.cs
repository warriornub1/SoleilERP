using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.PurchaseOrders
{
    internal class POHeaderRepository : GenericRepository<POHeader>, IPOHeaderRepository
    {
        public POHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckPoHeaderExisted(int id)
        {
            return await _dbContext.PoHeaders.AnyAsync(x => x.id == id);
        }
    }
}
