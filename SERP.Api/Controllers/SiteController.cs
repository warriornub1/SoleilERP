using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.Sites.DTOs.Request;
using SERP.Application.Masters.Sites.Services;
using SERP.Domain.Masters.Items.Model;
using SERP.Domain.Masters.Sites.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SiteController: ControllerBase
    {
        private readonly HttpContextService _httpContextService;
        private readonly ISiteService _siteService;
        private readonly IMapper _mapper;

        public SiteController(HttpContextService httpContextService,
            ISiteService siteService,
            IMapper mapper)
        {
            _httpContextService = httpContextService;
            _siteService = siteService;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult> GetAllLimited()
        {
            var sites = await _siteService.GetAllLimitedAsync();
            return Ok(sites);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Import([FromForm] ImportItemRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _siteService.ImportSiteAsync(userId, requestModel.File);
            return Ok(res);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateSite([FromBody] CreateSiteRequestModel model)
        {
            var request = _mapper.Map<CreateSiteRequestDto>(model);
            var result = await _siteService.CreateSiteAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok(new
            {
                id = result
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateSite([FromBody] UpdateSiteRequestModel model)
        {
            var request = _mapper.Map<UpdateSiteRequestDto>(model);
            await _siteService.UpdateSiteAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSite([FromRoute] int id)
        {
            await _siteService.DeleteSiteAsync(id);
            return Ok();
        }
    }
}
