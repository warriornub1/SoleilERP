using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.InboundShipments.Interfaces;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;

namespace SERP.Infrastructure.Transactions.InboundShipments
{
    public class InboundShipmentASNRepository : GenericRepository<InboundShipmentASN>, IInboundShipmentASNRepository
    {
        public InboundShipmentASNRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<InboundShipmentASN>> GetByConditionAsync(Expression<Func<InboundShipmentASN, bool>> predicate)
        {
            return await _dbContext.InboundShipmentASN.Where(predicate).ToListAsync();
        }
    }
}
