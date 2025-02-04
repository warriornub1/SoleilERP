using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Services;
using SERP.Application.Masters.Companies.DTOs.Request;
using SERP.Application.Masters.Companies.Services;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CompanyController: ControllerBase
    {
        private readonly HttpContextService _httpContextService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(HttpContextService httpContextService, ICompanyService companyService, IWebHostEnvironment webHostEnvironment
            , IDataRetrievalService dataRetrievalService)
        {
            _httpContextService = httpContextService;
            _companyService = companyService;
            _webHostEnvironment = webHostEnvironment;
            _dataRetrievalService = dataRetrievalService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _companyService.GetByIdAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllLimited(bool onlyEnabled)
        {
            var companies = await _companyService.GetAllLimitedAsync(onlyEnabled);
            return Ok(companies);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllPaged(int page, int pageSize)
        {
            var companies = await _companyService.GetAllPagedAsync(page, pageSize);
            return Ok(companies);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> SearchPaged(int page, int pageSize, string? keyword, [FromBody] SearchCompanyPagedRequestModel filter)
        {
            var companies = await _companyService.SearchPagedAsync(page, pageSize, keyword, filter);
            return Ok(companies);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateRequestDto request)
        {
            await _companyService.CreateCompanyAsync(request, _httpContextService.GetCurrentUserId());
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] List<UpdateCompanyRequestDto> requests)
        {
            await _companyService.UpdateCompanyAsync(requests, _httpContextService.GetCurrentUserId());
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpGet]
        public async Task<IActionResult> GetCompanyTreeView()
        {
            var companyTreeView = await _companyService.GetCompanyTreeViewAsync();
            return Ok(companyTreeView);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> DeleteCompany(DeleteCompanyRequestsDto requests)
        {
            await _companyService.DeleteCompanyAsync(requests);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetCompanyList()
        {
            var companyList = await _companyService.GetCompanyListAsync();
            return Ok(companyList);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> ImportCompany([FromForm] ImportCompanyRequestDto file)
        {
            await _companyService.ImportCompanyGroupAsync(_httpContextService.GetCurrentUserId(), file);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetCompanyTemplate()
        {
            var file = await _dataRetrievalService.GetTemplateAsync(_webHostEnvironment.WebRootPath, ApplicationConstant.wwwRootFileName.Company);
            Response.Headers.Add("Content-Disposition", "Content-Disposition");
            return new FileStreamResult(new MemoryStream(file), HttpMediaType.Excel)
            {
                FileDownloadName = ApplicationConstant.FileName.Company
            };
        }

    }
}
