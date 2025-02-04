using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.Suppliers.DTOs.Request;
using SERP.Application.Masters.Suppliers.Services;
using SERP.Domain.Masters.Suppliers.Models;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private HttpContextService _httpContextService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            _httpContextService = new HttpContextService();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllLimited(bool onlyEnabled)
        {
            var suppliers = await _supplierService.GetAllLimited(onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, suppliers);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var supplier = await _supplierService.GetById(id);
            return StatusCode(StatusCodes.Status200OK, supplier);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{supplierId:int}/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetSecondarySupplierLimited(int supplierId, bool onlyEnabled)
        {
            var suppliers = await _supplierService.GetSecondarySupplierLimited(supplierId, onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, suppliers);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{supplierId:int}/{itemId:int}/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetSupplierItemMapping(int supplierId, int itemId, bool onlyEnabled)
        {
            var supplierItems = await _supplierService.GetSupplierItemMapping(supplierId, itemId, onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, supplierItems);
        }

        [HttpPost]
        public async Task<IActionResult> ImportSupplier([FromForm] ImportSupplierRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _supplierService.ImportSupplierAsync(userId, requestModel.File);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ImportSupplierItemMapping([FromForm] ImportSupplierRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _supplierService.ImportSupplierItemAsync(userId, requestModel.File);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> ImportSupplierSecondary([FromForm] ImportSupplierRequestModel requestModel)
        {
            var userId = _httpContextService.GetCurrentUserId();
            var res = await _supplierService.ImportSupplierSecondaryAsync(userId, requestModel.File);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<CreateSupplierDto> model)
        {
            var res = await _supplierService.CreateSupplierAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok(new {supplier_id = res});
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] List<UpdateSupplierDto> model)
        {
            var userId = _httpContextService.GetCurrentUserId();
            await _supplierService.UpdateSupplierAsync(userId, model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] List<int> supplierIds)
        {
            await _supplierService.DeleteSupplierAsync(supplierIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] SearchPagedRequestDto request,
            [FromBody] FilterPagedSupplierRequestDto filter)
        {
            var result = await _supplierService.SearchPagedAsync(request, filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchItemMappingPaged(
            [FromQuery] SearchPagedRequestDto request,
            [FromBody] FilterPagedRelativeSupplierRequestDto filter)
        {
            var result = await _supplierService.SearchItemMappingPagedAsync(request, filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchSecondaryPaged(
            [FromQuery] SearchPagedRequestDto request,
            [FromBody] FilterPagedRelativeSupplierRequestDto filter)
        {
            var result = await _supplierService.SearchSecondaryPagedAsync(request, filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchIntermediaryPaged(
            [FromQuery] SearchPagedRequestDto request,
            [FromBody] FilterPagedRelativeSupplierRequestDto filter)
        {
            var result = await _supplierService.SearchIntermediaryPagedAsync(request, filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchSelfCollectSitePaged(
            [FromQuery] SearchPagedRequestDto request,
            [FromBody] FilterPagedSupplierSelfCollectSiteRequestDto filter)
        {
            var result = await _supplierService.SearchSelfCollectSitePagedAsync(request, filter);
            return Ok(result);
        }

        [HttpPut("{supplierId:int}")]
        public async Task<IActionResult> UpdateItemMapping([FromRoute] int supplierId, [FromBody] List<UpdateItemMappingRequestDto> model)
        {
            var userId = _httpContextService.GetCurrentUserId();
            await _supplierService.UpdateItemMappingAsync(userId, supplierId, model);
            return Ok();
        }

        [HttpPut("{supplierId:int}")]
        public async Task<IActionResult> UpdateSecondary([FromRoute] int supplierId, [FromBody] List<UpdateSecondarySupplierRequestDto> model)
        {
            var userId = _httpContextService.GetCurrentUserId();
            await _supplierService.UpdateSecondaryAsync(userId, supplierId, model);
            return Ok();
        }

        [HttpPut("{supplierId:int}")]
        public async Task<IActionResult> UpdateIntermediary([FromRoute] int supplierId, [FromBody] List<UpdateIntermediaryRequestDto> model)
        {
            var userId = _httpContextService.GetCurrentUserId();
            await _supplierService.UpdateIntermediaryAsync(userId, supplierId, model);
            return Ok();
        }

        [HttpPut("{supplierId:int}")]
        public async Task<IActionResult> UpdateSelfCollectSite([FromRoute] int supplierId, [FromBody] List<UpdateSupplierSelfCollectSiteRequestDto> model)
        {
            var userId = _httpContextService.GetCurrentUserId();
            await _supplierService.UpdateSelfCollectSiteAsync(userId, supplierId, model);
            return Ok();
        }
    }
}
