using Microsoft.AspNetCore.Http;
using SERP.Application.Masters.Countries.DTOs.Response;

namespace SERP.Application.Masters.Countries.Services
{
    public interface ICountryService
    {
        Task<List<CountryResponseDto>> GetAllLimitedAsync();

        Task<object> ImportCountryAsync(string userId, IFormFile file);
    }
}
