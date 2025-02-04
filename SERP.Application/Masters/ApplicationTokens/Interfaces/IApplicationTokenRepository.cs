using SERP.Application.Common;
using SERP.Domain.Masters.ApplicationTokens;

namespace SERP.Application.Masters.ApplicationTokens.Interfaces
{
    public interface IApplicationTokenRepository : IGenericRepository<ApplicationToken>
    {
        Task<ApplicationToken> GetToken(string applicationCode);
    }
}
