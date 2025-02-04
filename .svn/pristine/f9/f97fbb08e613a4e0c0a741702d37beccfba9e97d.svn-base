using Microsoft.AspNetCore.Http;
using SERP.Application.Masters.Ports.DTOs.Request;
using SERP.Application.Masters.Ports.DTOs.Response;

namespace SERP.Application.Masters.Ports.Services
{
    public interface IPortService
    {
        /// <summary>
        /// use country_alpha_code_two, or countryId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<PortResponseDto>> GetByCountryCode(GetPortRequestDto request);

        Task<object> ImportPortAsync(string userId, IFormFile file);
    }
}
