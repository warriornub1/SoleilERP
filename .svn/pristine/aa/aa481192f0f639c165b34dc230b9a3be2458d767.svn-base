using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.Natural_Accounts.DTOs.Request;
using SERP.Application.Finance.Natural_Accounts.Services;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NaturalAccountController : ControllerBase
    {
        private readonly INaturalAccountService _naturalAccountService;
        private readonly HttpContextService _httpContextService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NaturalAccountController(INaturalAccountService naturalAccountService, HttpContextService httpContextService, IWebHostEnvironment webHostEnvironment)
        {
            _naturalAccountService = naturalAccountService;
            _httpContextService = httpContextService;
            _webHostEnvironment = webHostEnvironment;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetAllPaged(int page, int pageSize)
        {
            var costCenter = await _naturalAccountService.GetAllPagedAsync(page, pageSize);
            return Ok(costCenter);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> SearchPaged(int page, int pageSize, string? keyword, [FromBody] SearchNaturalAccountRequestModel filter)
        {
            var costCenters = await _naturalAccountService.SearchPagedAsync(page, pageSize, keyword, filter);
            return Ok(costCenters);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _naturalAccountService.GetByIdAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateNaturalAccount(CreateNaturalAccountRequestModel request)
        {
            await _naturalAccountService.CreateNaturalAccountAsync(request, _httpContextService.GetCurrentUserId());
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetNaturalAccountTemplate()
        {
            var file = await _naturalAccountService.GetNaturalAccountTemplate(_webHostEnvironment.WebRootPath);
            Response.Headers.Add("Content-Disposition", "Content-Disposition");
            return new FileStreamResult(new MemoryStream(file), HttpMediaType.Excel)
            {
                FileDownloadName = ApplicationConstant.FileName.NaturalAccount
            };
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> UpdateNaturalAccount(List<UpdateNaturalAccountRequestModel> requests)
        {
            await _naturalAccountService.UpdateNaturalAccountAsync(requests, _httpContextService.GetCurrentUserId());
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> DeleteNaturalAccount(DeleteNaturalAccountRequestModel request)
        {
            await _naturalAccountService.DeleteNaturalAccountAsync(request);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> ImportNaturalAccount([FromForm] ImportNaturalAccountRequestModel request)
        {
            await _naturalAccountService.ImportNaturalAccountAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetNaturalAccountTreeView()
        {
            var result = await _naturalAccountService.GetNaturalAccountTreeViewAsync();
            return Ok(result);
        }
    }
}
