using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Api.Common.FileServers;
using SERP.Application.Common.Dto;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Application.Transactions.Receiving.DTOs.Request;
using SERP.Application.Transactions.Receiving.DTOs.Response;
using SERP.Application.Transactions.Receiving.Services;
using SERP.Domain.Common.Constants;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]

    public class ReceivingController : ControllerBase
    {
        private readonly IReceivingService _receivingService;
        private readonly HttpContextService _httpContextService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ReceivingController(
            IReceivingService receivingService,
            HttpContextService httpContextService,
            IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _receivingService = receivingService;
            _httpContextService = httpContextService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceiving([FromBody] CreateReceivingRequestDto request)
        {
            int rcvHeaderId = await _receivingService.CreateReceivingAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok( rcvHeaderId );
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateReceiving([FromBody] UpdateReceivingRequestDto request)
        {
            await _receivingService.UpdateReceivingAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }

        [HttpDelete("{receiving_detail_id:int}")]
        public async Task<IActionResult> DeleteReceivingLine([FromRoute] int receiving_detail_id)
        {
            await _receivingService.DeleteReceivingLineAsync(_httpContextService.GetCurrentUserId(), receiving_detail_id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SearchPaged([FromQuery] SearchPagedRequestDto searchPagedDto, [FromBody] PagedFilterReceivingRequestDto filter)
        {
            var result = await _receivingService.PagedFilterReceivingAsync(searchPagedDto, filter);
            return Ok(result);
        }
        
        [HttpGet("{receiving_header_id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int receiving_header_id)
        {
            var rcvDetail = await _receivingService.GetByReceivingHeaderIdAsync(receiving_header_id);
            var result = _mapper.Map<ReceivingGetByIdFileBinaryResponseDTO>(rcvDetail);

            foreach (var detail in result.receiving_details)
            {
                List<RcvFileDetail> detailFile = rcvDetail.photos.Where(x => x.receiving_detail_id == detail.receiving_detail_id).ToList();
                foreach (var file in detailFile)
                {
                    detail.photos.Add(new RcvFile
                    {
                        file_id = file.id,
                        file_type = file.file_type,
                        photo = FileServerHelper.ConvertFileToIFormFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.RECEVINGFile, file.url_path)
                    });
                }
            }

            return Ok(result);
        }
        
        [HttpGet("{receivingNo}")]
        public async Task<IActionResult> GetByRecevingNo([FromRoute] string receivingNo)
        {
            var rcvDetail = await _receivingService.GetByReceivingNoAsync(receivingNo);
            var result = _mapper.Map<ReceivingGetByIdFileBinaryResponseDTO>(rcvDetail);

            foreach (var detail in result.receiving_details)
            {
                List<RcvFileDetail> detailFile = rcvDetail.photos.Where(x => x.receiving_detail_id == detail.receiving_detail_id).ToList();
                foreach (var file in detailFile)
                {
                    detail.photos.Add(new RcvFile
                    {
                        file_id = file.id,
                        file_type = file.file_type,
                        photo = FileServerHelper.ConvertFileToIFormFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.RECEVINGFile, file.url_path)
                    });
                }
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetReceivingList([FromBody] GetReceivingListRequestDto request)
        {
            var result = await _receivingService.GetReceivingListAsync(request);

            return Ok(result);
        }
        
        [HttpGet("{receiving_header_id:int}")]
        public async Task<IActionResult> GetDocumentList([FromRoute] int receiving_header_id)
        {
            var result = await _receivingService.GetDocumentList(receiving_header_id);

            return Ok(result);
        }
        
        [HttpGet("{receiving_header_id:int}")]
        public async Task<IActionResult> GetItemList([FromRoute] int receiving_header_id, string? document_no, string? package_no)
        {
            var result = await _receivingService.GetItemListAsync(receiving_header_id, document_no, package_no);

            return Ok(result);
        }
        
        [HttpGet("{receiving_header_id:int}/{item_id:int}")]
        public async Task<IActionResult> GetReceivingDetailLineByItem([FromRoute] int receiving_header_id, int item_id)
        {
            var result = await _receivingService.GetReceivingDetailLineByItemAsync(receiving_header_id,item_id);

            return Ok(result);
        }
       
        [HttpGet("{receiving_detail_id:int}")]
        public async Task<IActionResult> GetItemDetails([FromRoute] int receiving_detail_id)
        {
            var result = await _receivingService.GetItemDetailsAsync(receiving_detail_id);

            return Ok(result);
        }
        
        [HttpPost("{receiving_header_id:int}")]
        public async Task<IActionResult> SearchDetail([FromRoute] int receiving_header_id, string? keyword,[FromBody] GetReceivingListRequestDto request)
        {
            var rcvDetail = await _receivingService.SearchDetailAsync(receiving_header_id, keyword, request);
            var result = _mapper.Map<ReceivingDetailFileResponseDto>(rcvDetail);

            foreach(var detail in result.receiving_details)
            {
                List<RcvFileDetail> detailFile = rcvDetail.photos.Where(x=>x.receiving_detail_id == detail.receiving_detail_id).ToList();
                foreach (var file in detailFile)
                {
                    detail.photos.Add(new RcvFile
                    {
                        file_id = file.id,
                        file_type = file.file_type,
                        photo = FileServerHelper.ConvertFileToIFormFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.RECEVINGFile, file.url_path)
                    });
                }
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateByAction([FromBody] UpdateReceivingByActionDto request)
        {
            await _receivingService.UpdateByActionAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }
        [HttpPut("{receiving_header_id:int}")]
        public async Task<IActionResult> DeleteReceiving([FromRoute] int receiving_header_id)
        {
            await _receivingService.DeleteReceivingAsync(_httpContextService.GetCurrentUserId(), receiving_header_id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadReceivingFileDto request)
        {
            var fileInfos = new List<FileInfoRequestDto>();
            foreach (var file in request.files)
            {
                fileInfos.Add(new FileInfoRequestDto
                {
                    file = file,
                    url_path = FileServerHelper.SaveToResourceFolder(_webHostEnvironment.ContentRootPath,
                                file, Resources.RECEVINGFile)
                });
            }

            try
            {
                await _receivingService.UploadFileAsync(_httpContextService.GetCurrentUserId(), request.receiving_header_id, request.receiving_detail_id, fileInfos);
                return Ok();
            }
            catch
            {
                foreach (var item in fileInfos)
                {
                    if (!string.IsNullOrEmpty(item.url_path))
                    {
                        FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, Resources.RECEVINGFile, item.url_path);
                    }
                }
                throw;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveFile([FromBody] DeleteReceivingFileDto request)
        {
            var fileInfos = new List<FileInfoRequestDto>();

            var deletedFilePath = await _receivingService.DeleteFilesAsync(_httpContextService.GetCurrentUserId(), request);

            foreach (var path in deletedFilePath)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    FileServerHelper.RemoveResourceFile(_webHostEnvironment.ContentRootPath, DomainConstant.Resources.CONTAINERFile, path);
                }
            }
            return Ok();
        }
    }
}
