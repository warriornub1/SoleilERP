using SERP.Application.Transactions.ApplicationToken.Response;

namespace SERP.Application.Masters.ApplicationTokens.Services
{
    public interface IApplicationTokenService
    {
        Task<ApplicationTokenDto> GetByApplicationCode(string userId, string applicationCode);
        Task CreateTokenAsync(string userId);
    }
}
