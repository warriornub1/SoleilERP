using Microsoft.AspNetCore.Http;
using SERP.Application.Masters.Sites.DTOs.Request;
using SERP.Application.Masters.Sites.DTOs.Response;

namespace SERP.Application.Masters.Sites.Services
{
    public interface ISiteService
    {
        Task<List<SiteResponseDto>> GetAllLimitedAsync();
        Task<object> ImportSiteAsync(string userId, IFormFile file);
        Task<int> CreateSiteAsync(string userId, CreateSiteRequestDto request);
        Task UpdateSiteAsync(string userId, UpdateSiteRequestDto request);
        Task DeleteSiteAsync(int id);
    }
}
