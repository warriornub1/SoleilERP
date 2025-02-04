using SERP.Application.Common;
using SERP.Domain.Masters.Agents;
using SERP.Domain.Masters.Agents.Model;

namespace SERP.Application.Masters.Agents.Interfaces
{
    public interface IAgentRepository : IGenericRepository<Agent>
    {
        Task<IEnumerable<Agent>> GetByAgentType(string agentType, bool onlyEnabled);
        Task<int[]> GetAvailableAgentIds(HashSet<int> toHashSet);
        Task<bool> CheckExistedAsync(string requestAgentNo);
        Task<string[]> CheckInvalidAgentNOs(List<Agent> agentNo);
        IQueryable<PagedAgentDetail> BuildFilterAgentQuery(PagedFilterAgentRequestModel pagedFilterAgentRequestModel);
        Task<List<Agent>> GetListAgentAsync(List<int> agentIds);
    }
}
