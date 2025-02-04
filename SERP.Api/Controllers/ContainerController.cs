using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Transactions.Containers.DTOs.Request;
using SERP.Application.Transactions.Containers.Services;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]

    public class ContainerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContainerService _containerService;
        private readonly HttpContextService _httpContextService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContainerController(
            IContainerService containerService,
            HttpContextService httpContextService,
            IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _containerService = containerService;
            _httpContextService = httpContextService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer([FromBody] List<CreateContainerRequestDto> request)
        {
            var result = await _containerService.CreateContainerAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok(new
            {
                id = result
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteContainer([FromQuery] int containerIds)
        {
            await _containerService.DeleteContainerAsync(_httpContextService.GetCurrentUserId(), containerIds);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPaged([FromQuery] SearchPagedRequestDto searchPagedDto, [FromBody] PagedFilterContainerRequestDto filter)
        {
            var result = await _containerService.PagedFilterContainerAsync(searchPagedDto, filter);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContainerByAction([FromForm] UpdateContainerByActionRequestDto request)
        {
            //var fileInfos = new List<FileInfoRequestDto>();
            //if(request.files != null)
            //{
            //    foreach (var file in request.files)
            //    {
            //        fileInfos.Add(new FileInfoRequestDto
            //        {
            //            file = file,
            //            url_path = FileServerHelper.SaveToResourceFolder(_webHostEnvironment.ContentRootPath, file,
            //                DomainConstant.Resources.CONTAINERFile)
            //        });
            //    }
            //}

            //var deletedFilePath = await _containerService.UpdateContainerByActionAsync(_httpContextService.GetCurrentUserId(), _httpContextService.GetCurrentBranchPlant(), 
            //    _webHostEnvironment.ContentRootPath, request);
            await _containerService.UpdateContainerByActionAsync(_httpContextService.GetCurrentUserId(), _httpContextService.GetCurrentBranchPlant(),
                 _webHostEnvironment.ContentRootPath, request);

            //foreach (var path in deletedFilePath)
            //{
            //    if (!string.IsNullOrEmpty(path))
            //    {
            //        FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.CONTAINERFile, path);
            //    }
            //}
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateContainer([FromBody] List<UpdateContainerRequestDto> request)
        {
            await _containerService.UpdateContainerAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var container = await _containerService.GetByIdAsync(id);

            return Ok(container);
        }
        [HttpPost("{bpNo}")]
        public async Task<IActionResult> GetContainerList([FromRoute] string bpNo, [FromBody] GetContainerListRequestDto request)
        {
            var result = await _containerService.GetContainerListAsync(bpNo, request);

            return Ok(result);
        }
        //[HttpPost]
        //public async Task<IActionResult> UploadFile([FromForm] UploadContainerFileDto request)
        //{
        //    var fileInfos = new List<ContainerFileInfoRequestDto>();
        //    foreach (var file in request.files)
        //    {
        //        fileInfos.Add(new ContainerFileInfoRequestDto
        //        {
        //            file = file.file,
        //            url_path = FileServerHelper.SaveToResourceFolder(_webHostEnvironment.ContentRootPath, file.file,
        //                DomainConstant.Resources.CONTAINERFile),
        //            container_file_type=file.container_file_type
        //        });
        //    }

        //    try
        //    {
        //        await _containerService.UploadFileAsync(_httpContextService.GetCurrentUserId(), request.container_id, fileInfos);
        //        return Ok();
        //    }
        //    catch
        //    {
        //        foreach (var item in fileInfos)
        //        {
        //            if (!string.IsNullOrEmpty(item.url_path))
        //            {
        //                FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.CONTAINERFile, item.url_path);
        //            }
        //        }
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> RemoveFile([FromBody] RemoveContainerFileDto request)
        //{
        //    var filePath = await _containerService.RemoveFileAsync(request);

        //    foreach (var path in filePath)
        //    {
        //        if (!string.IsNullOrEmpty(path))
        //        {
        //            FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.CONTAINERFile, path);
        //        }
        //    }

        //    return Ok();
        //}
    }
}
