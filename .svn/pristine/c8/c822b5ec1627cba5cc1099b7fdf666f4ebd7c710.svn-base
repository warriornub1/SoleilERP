using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.BranchPlants.DTOs.Request;
using SERP.Application.Masters.BranchPlants.Services;
using SERP.Domain.Masters.BranchPlants.Models;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BranchPlantController : ControllerBase
    {
        private readonly IBranchPlantService _branchPlantService;
        private HttpContextService _httpContextService;
        private readonly IMapper _mapper;

        public BranchPlantController(IBranchPlantService branchPlantService,
            HttpContextService httpContextService,
            IMapper mapper)
        {
            _branchPlantService = branchPlantService;
            _httpContextService = httpContextService;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var branchPlant = await _branchPlantService.GetById(id);
            return StatusCode(StatusCodes.Status200OK, branchPlant);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{companyNo}")]
        public async Task<ActionResult> GetByCompany(string companyNo)
        {
            var branchPlants = await _branchPlantService.GetByCompanyAsync(companyNo);
            return StatusCode(StatusCodes.Status200OK, branchPlants);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateBranchPlant([FromBody] List<CreateBranchPlantRequestModel> model)
        {
            var request = _mapper.Map <List<CreateBranchPlantRequestDto>>(model);
            var result = await _branchPlantService.CreateAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok(new
            {
                id = result
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> UpdateBranchPlant([FromBody] List<UpdateBranchPlantRequestModel> model)
        {
            var request = _mapper.Map <List<UpdateBranchPlantRequestDto>>(model);
            await _branchPlantService.UpdateAsync(_httpContextService.GetCurrentUserId(), request);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<int> ids)
        {
            await _branchPlantService.DeleteAsync(ids);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBranchPlant([FromRoute] int id)
        {
            await _branchPlantService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult SearchPaged([FromQuery] SearchPagedRequestDto searchPagedDto, [FromBody] PagedFilterBranchPlantRequestDto filter)
        {
            var result = _branchPlantService.PagedFilterBranchPlantAsync(searchPagedDto, filter);
            return Ok(result);
        }
    }
}
