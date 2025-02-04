using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.Ports.DTOs.Request;
using SERP.Application.Masters.Ports.Services;
using SERP.Domain.Masters.Items.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PortController : ControllerBase
    {
        private readonly HttpContextService _httpContextService;
        private readonly IPortService _portService;

        public PortController(HttpContextService httpContextService,
            IPortService portService)
        {
            _httpContextService = httpContextService;
            _portService = portService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetByCountryCode([FromQuery] GetPortRequestModel model)
        {
            var result = await _portService.GetByCountryCode(new GetPortRequestDto
            {
                CountryCode = model.CountryCode,
                CountryId = model.CountryId
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Import([FromForm] ImportItemRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _portService.ImportPortAsync(userId, requestModel.File);
            return Ok(res);
        }
    }
}
