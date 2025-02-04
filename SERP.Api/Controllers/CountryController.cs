using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.Countries.Services;
using SERP.Domain.Masters.Items.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly HttpContextService _httpContextService;

        public CountryController(ICountryService countryService,
        HttpContextService httpContextService)
        {
            _countryService = countryService;
            _httpContextService = httpContextService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetAllLimited()
        {
            var countries = await _countryService.GetAllLimitedAsync();
            return Ok(countries);
        }

        [HttpPost]
        public async Task<IActionResult> Import([FromForm] ImportItemRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _countryService.ImportCountryAsync(userId, requestModel.File);
            return Ok(res);
        }
    }
}
