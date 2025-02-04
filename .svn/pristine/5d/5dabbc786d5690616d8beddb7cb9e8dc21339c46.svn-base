using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.ApplicationTokens.Interfaces;
using SERP.Domain.Masters.ApplicationTokens;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.ApplicationTokens
{
    public class ApplicationTokenRepository : GenericRepository<ApplicationToken>, IApplicationTokenRepository
    {
        public ApplicationTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ApplicationToken> GetToken(string applicationCode)
        {
            return await _dbContext.ApplicationToken.Where(x => x.application_code.Equals(applicationCode))
                                                    .FirstOrDefaultAsync();
        }

    }
}
