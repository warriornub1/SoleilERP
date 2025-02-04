using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Api.Common.FileServers;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.SystemKVSs.Services;
using SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request;
using SERP.Application.Transactions.AdvancedShipmentNotices.Services;
using SERP.Application.Transactions.Invoices.DTOs.Request;
using SERP.Application.Transactions.Invoices.Service;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Common.Constants;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly HttpContextService _httpContextService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISystemKvsService _systemKvsService;

        public InvoiceController(
            HttpContextService httpContextService,
            IInvoiceService invoiceService,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            ISystemKvsService systemKvsService)
        {
            _httpContextService = httpContextService;
            _invoiceService = invoiceService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _systemKvsService = systemKvsService;
        }

        [HttpPost]
        public IActionResult SearchPaged(
            [FromQuery] SearchPagedRequestDto model,
            [FromBody] FilterInvoicePagedRequestDto filter)
        {
            var result = _invoiceService.SearchPaged(model, filter);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SearchDetail(
            [FromQuery] SearchPagedRequestDto model,
            [FromBody] FilterInvoiceDetailRequestDto filter)
        {
            var result = _invoiceService.SearchDetail(model, filter);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _invoiceService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{invoiceNo}")]
        public async Task<IActionResult> GetByInvoiceNo([FromRoute] string invoiceNo)
        {
            var result = await _invoiceService.GetByInvoiceNoAsync(invoiceNo);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequestDto request)
        {
            var result = await _invoiceService.CreateInvoiceAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok(new
            {
                ids = result
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceRequestDto request)
        {
            await _invoiceService.UpdateInvoiceAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int id)
        {
            await _invoiceService.DeleteInvoiceAsync(_httpContextService.GetCurrentUserId(), id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadInvoiceRequestModel model)
        {
            await _systemKvsService.ValidateFileUploadAsync(model.files);

            var request = _mapper.Map<UploadInvoiceFileRequestDto>(model);

            var fileInfos = new List<FileInfoRequestDto>();
            foreach (var file in model.files)
            {
                fileInfos.Add(new FileInfoRequestDto
                {
                    file = file,
                    url_path = FileServerHelper.SaveToResourceFolder(_webHostEnvironment.ContentRootPath, file,
                        DomainConstant.Resources.InvoiceFile)
                });
            }

            request.files = fileInfos;
            try
            {
                var result = await _invoiceService.UploadFileAsync(_httpContextService.GetCurrentUserId(), request);

                return Ok(new
                {
                    InvoiceFileIDs = result
                });
            }
            catch
            {
                foreach (var item in fileInfos)
                {
                    if (!string.IsNullOrEmpty(item.url_path))
                    {
                        FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.InvoiceFile, item.url_path);
                    }
                }
                throw;
            }
        }

        [HttpPost("{invoiceHeaderId:int}")]
        public async Task<IActionResult> RemoveFile([FromRoute] int invoiceHeaderId, [FromBody] List<int> invoiceFileIDs)
        {
            var filePath = await _invoiceService.RemoveFileAsync(invoiceHeaderId, invoiceFileIDs);

            foreach (var path in filePath)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.InvoiceFile, path);
                }
            }

            return Ok();
        }
    }
}
