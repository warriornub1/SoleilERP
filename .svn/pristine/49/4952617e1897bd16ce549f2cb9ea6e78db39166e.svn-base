using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Common.Dto;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.Agents.Services;
using SERP.Domain.Masters.Agents.Model;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly HttpContextService _httpContextService;
        private readonly IMapper _mapper;

        public AgentController(IAgentService agentService,
            HttpContextService httpContextService,
            IMapper mapper)
        {
            _agentService = agentService;
            _httpContextService = httpContextService;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{agentType}/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetByAgentType(string agentType, bool onlyEnabled)
        {
            var agents = await _agentService.GetByAgentType(agentType, onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, agents);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<CreateAgentRequestDto> model)
        {
            var result = await _agentService.CreateAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok(new
            {
                id = result
            });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] List<UpdateAgentRequestDto> model)
        {
            await _agentService.UpdateAsync(_httpContextService.GetCurrentUserId(), model);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<int> ids)
        {
            await _agentService.DeleteAsync(ids);
            return Ok();
        }

        [HttpPost]
        public IActionResult SearchPaged([FromQuery] SearchPagedRequestDto searchPagedDto,[FromBody] PagedFilterAgentRequestDto filter)
        {
            var result = _agentService.PagedFilterAgentAsync(searchPagedDto, filter);
            return Ok(result);
        }
    }
}
