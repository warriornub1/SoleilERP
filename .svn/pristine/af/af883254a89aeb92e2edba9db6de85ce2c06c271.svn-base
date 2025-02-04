using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Transactions.CustomViews.DTOs.Request;
using SERP.Application.Transactions.CustomViews.Services;
using SERP.Domain.Transactions.CustomViews.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomViewController : ControllerBase
    {
        private readonly ICustomViewService _customViewService;
        private readonly HttpContextService _httpContextService;
        private readonly IMapper _mapper;

        public CustomViewController(
            ICustomViewService customViewService,
            HttpContextService httpContextService,
            IMapper mapper)
        {
            _customViewService = customViewService;
            _httpContextService = httpContextService;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetByCustomViewType(string customViewType, string? userId)
        {
            var result = await _customViewService.GetByCustomViewType(customViewType, userId);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAttributesByCustomViewId(int customViewId)
        {
            var result = await _customViewService.GetAttributesByCustomViewId(customViewId);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateCustomView([FromBody] CreateCustomViewRequestModel model)
        {
            var request = _mapper.Map<CreateCustomViewRequestDto>(model);
            var result = await _customViewService.CreateCustomView(_httpContextService.GetCurrentUserId(), request);
            return Ok(new
            {
                id = result
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomViewAttributes([FromBody] UpdateCustomViewAttributeRequestDto model)
        {
            await _customViewService.UpdateCustomViewAttributes(_httpContextService.GetCurrentUserId(), model);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomView([FromBody] List<int> customViewIds)
        {
            await _customViewService.DeleteCustomView(customViewIds);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomView([FromBody] List<UpdateCustomViewRequestDto> models)
        {
            await _customViewService.UpdateCustomView(_httpContextService.GetCurrentUserId(), models);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public IActionResult SearchPaged([FromQuery] SearchPagedRequestDto model, [FromBody] FilterCustomViewPagedRequestDto filter)
        {
            var result = _customViewService.SearchPaged(model, filter);
            return Ok(result);
        }
    }
}
