using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Services;
using SERP.Application.Finance.Groups.DTOs.Request;
using SERP.Application.Finance.Groups.Services;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private HttpContextService _httpContextService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDataRetrievalService _dataRetrievalService;
        private IMapper _mapper;
        
        public GroupController(IGroupService groupService, IMapper mapper, IConfiguration configuration, IWebHostEnvironment webHostEnvironment,
            IDataRetrievalService dataRetrievalService)
        {
            _groupService = groupService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextService = new HttpContextService();
            _dataRetrievalService = dataRetrievalService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPaged([FromQuery] int page, int pageSize, string groupType)
        {
            var item = await _groupService.GetAllPagedAsync(page, pageSize, groupType);
            return Ok(item);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> SearchPaged([FromQuery] SearchGroupPagedRequestModel model,
                                                        [FromBody] SearchPagedGroupRequestModel filter)
        {
            var item = await _groupService.SearchPagedAsync(model.Page, model.PageSize, model.groupType, model.Keyword, filter);
            return Ok(item);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _groupService.GetByIdAsync(id);
            return Ok(item);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequestModel model)
        {
            await _groupService.CreateGroupAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateGroup([FromBody] List<UpdateGroupRequestModel> model)
        {
            await _groupService.UpdateGroupAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteGroup([FromBody] DeleteGroupRequestModel request)
        {
            await _groupService.DeleteGroupAsync(request);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> ImportCompanyGroup([FromForm] ImportCompanyGroupModel file)
        {
            await _groupService.ImportCompanyGroupAsync(_httpContextService.GetCurrentUserId(), file);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> ImportCostCenterGroup([FromForm] ImportCompanyGroupModel file)
        {
            await _groupService.ImportCostCenterGroupAsync(_httpContextService.GetCurrentUserId(), file);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> ImportRevenueCenterGroup([FromForm] ImportCompanyGroupModel file)
        {
            await _groupService.ImportRevenueCenterGroupAsync(_httpContextService.GetCurrentUserId(), file);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("[action]")]
        public async Task<IActionResult> ImportNaturalAccountGroup([FromForm] ImportCompanyGroupModel file)
        {
            await _groupService.ImportNaturalAccountGroupAsync(_httpContextService.GetCurrentUserId(), file);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetGroupListByGroupType(string groupType)
        {
            var result = await _groupService.GetGroupListByGroupTypeAsync(groupType);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetParentGroupListByGroupType(string groupType)
        {
            var result = await _groupService.GetParentGroupListByGroupTypeAsync(groupType);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetGroupTemplate()
        {
            var file = await _dataRetrievalService.GetTemplateAsync(_webHostEnvironment.WebRootPath, ApplicationConstant.wwwRootFileName.Group);
            return new FileStreamResult(new MemoryStream(file), HttpMediaType.Excel)
            {
                FileDownloadName = ApplicationConstant.FileName.Group
            };
        }
    }
}
