using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.Items.Services;
using SERP.Domain.Masters.Items.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private HttpContextService _httpContextService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
            _httpContextService = new HttpContextService();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllLimited(bool onlyEnabled)
        {
            var items = await _itemService.GetAllLimited(onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, items);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var item = await _itemService.GetById(id);
            return StatusCode(StatusCodes.Status200OK, item);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Import([FromForm] ImportItemRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _itemService.ImportItemAsync(userId, requestModel.File);
            return Ok(res);
        }
    }
}
