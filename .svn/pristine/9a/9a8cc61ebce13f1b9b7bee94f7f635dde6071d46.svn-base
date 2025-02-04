using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Api.Common.FileServers;
using SERP.Application.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.SystemKVSs.Interfaces;
using SERP.Application.Masters.SystemKVSs.Services;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Application.Transactions.PurchaseOrders.Services;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;
using SERP.Domain.Transactions.PurchaseOrders.Model;
using SERP.Domain.Transactions.PurchaseOrders.Model.Base;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HttpContextService _httpContextService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISystemKvsService _systemKvsService;

        public PurchaseOrderController(
            IMapper mapper,
            HttpContextService httpContextService,
            IPurchaseOrderService purchaseOrderService,
            IWebHostEnvironment webHostEnvironment,
            ISystemKvsService systemKvsService)
        {
            _mapper = mapper;
            _httpContextService = httpContextService;
            _purchaseOrderService = purchaseOrderService;
            _webHostEnvironment = webHostEnvironment;
            _systemKvsService = systemKvsService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] SearchPagedRequestDto model,
            [FromBody] POFilterRequestDto filter)
        {
            var request = _mapper.Map<PagedFilterPoRequestDto>(model);
            _mapper.Map(filter, request);
            var result = await _purchaseOrderService.PagedFilterPoAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<object> GetAllPaged([FromQuery] PagedPORequestDto model)
        {
            var request = _mapper.Map<PagedFilterPoRequestDto>(model);
            var result = await _purchaseOrderService.PagedFilterPoAsync(request);
            return result;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _purchaseOrderService.GetByIdAsync(id);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{poNo}")]
        public async Task<IActionResult> GetByPoNo([FromRoute] string poNo)
        {
            var result = await _purchaseOrderService.GetByPoNoAsync(poNo);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreatePO([FromForm] CreatePoRequestDto model)
        {
            if (model.files is not null && model.files.Count > 0)
            {
                await _systemKvsService.ValidateFileUploadAsync(model.files);
            }

            var result = await _purchaseOrderService.CreatePoAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok(new
            {
                ids = result
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> UpdatePO([FromBody] UpdatePoRequestDto model)
        {
            await _purchaseOrderService.UpdatePoAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequestModel model)
        {
            await _systemKvsService.ValidateFileUploadAsync(model.files);

            var request = _mapper.Map<UploadFileRequestDto>(model);

            var fileInfos = new List<FileInfoRequestDto>();
            foreach (var file in model.files)
            {
                fileInfos.Add(new FileInfoRequestDto
                {
                    file = file,
                    url_path = FileServerHelper.SaveToResourceFolder(_webHostEnvironment.ContentRootPath, file,
                        DomainConstant.Resources.POFile)
                });
            }

            request.files = fileInfos;
            try
            {
                var result = await _purchaseOrderService.UploadFileAsync(_httpContextService.GetCurrentUserId(), request);
                return Ok(new
                {
                    PoFileIDs = result
                });
            }
            catch
            {
                foreach (var item in fileInfos)
                {
                    if (!string.IsNullOrEmpty(item.url_path))
                    {
                        FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.POFile, item.url_path);
                    }
                }
                throw;
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("{poHeaderId:int}")]
        public async Task<IActionResult> RemoveFile([FromRoute] int poHeaderId, [FromBody] List<int> poFileIDs)
        {
            var filePath = await _purchaseOrderService.RemoveFileAsync(poHeaderId, poFileIDs);

            foreach (var path in filePath)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.POFile, path);
                }
            }

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("{poHeaderId:int}")]
        public async Task<IActionResult> DeletePOLine([FromRoute] int poHeaderId, [FromBody] List<int> poDetailIDs)
        {
            await _purchaseOrderService.DeletePOLineAsync(_httpContextService.GetCurrentUserId(),
                new DeletePoLineRequestDto
                {
                    PoHeaderId = poHeaderId,
                    PoDetailIDs = poDetailIDs
                });

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePO([FromRoute] int id)
        {
            await _purchaseOrderService.DeletePOAsync(_httpContextService.GetCurrentUserId(), id);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> GetPoNoList(
            [FromQuery] string branchPlantNo,
            [FromQuery] bool onlyWithOpenQty,
            [FromBody] PoNoFilterDto filter)
        {
            var result = await _purchaseOrderService.GetPoNoListAsync(new PoNoRequestDto()
            {
                Suppliers = filter.Suppliers,
                branchPlantNo = branchPlantNo,
                onlyWithOpenQty = onlyWithOpenQty,
                Status = filter.Statuses
            });
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> SearchDetailPaged(
            [FromQuery] SearchPagedRequestDto model,
            [FromBody] SearchPoDetailRequestDto filter)
        {
            var request = _mapper.Map<PagedFilterPoRequestDto>(model);
            _mapper.Map(filter, request);
            var result = await _purchaseOrderService.SearchDetailPaged(request);
            return Ok(result);
        }

        [HttpPut("{poHeaderId:int}")]
        public async Task<IActionResult> ClosePO([FromRoute] int poHeaderId)
        {
            await _purchaseOrderService.ClosePoAsync(_httpContextService.GetCurrentUserId(), poHeaderId);
            return Ok();
        }

        [HttpPut("{poHeaderId:int}")]
        public async Task<IActionResult> ClosePOLine([FromRoute] int poHeaderId, [FromBody] List<int> poDetailIDs)
        {
            await _purchaseOrderService.ClosePoLineAsync(_httpContextService.GetCurrentUserId(), new DeletePoLineRequestDto()
            {
                PoHeaderId = poHeaderId,
                PoDetailIDs = poDetailIDs
            });
            return Ok();
        }
    }
}