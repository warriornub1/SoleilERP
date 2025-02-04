using SERP.Application.Common.Dto;
using SERP.Application.Masters.Agents.DTOs;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.Agents.DTOs.Response;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.Agents.Services
{
    public interface IAgentService
    {
        Task<IEnumerable<AgentDto>> GetByAgentType(string agentType, bool onlyEnabled);
        Task<List<int>> CreateAsync(string userId, List<CreateAgentRequestDto> requests);
        Task UpdateAsync(string userId, List<UpdateAgentRequestDto> requests);
        Task DeleteAsync(List<int> ids);
        PagedResponse<PagedAgentResponseDto> PagedFilterAgentAsync(SearchPagedRequestDto requests, PagedFilterAgentRequestDto filters);
    }
}
