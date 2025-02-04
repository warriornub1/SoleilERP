using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Agents.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.Agents;
using SERP.Domain.Masters.Agents.Model;
using SERP.Domain.Masters.Suppliers;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Agents
{
    public class AgentRepository : GenericRepository<Agent>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Agent>> GetByAgentType(string agentType, bool onlyEnabled)
        {
            return await _dbContext.Agent.Where(x => x.agent_type.Equals(agentType) && (!onlyEnabled || x.status_flag.Equals(ApplicationConstant.StatusFlag.Enabled))).ToListAsync();
        }

        public async Task<int[]> CheckInvalidAgentIds(HashSet<int> agentIds)
        {
            var query = await _dbContext.Agent
                .Where(x => agentIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
            var result = agentIds.Except(query).ToArray();
            return result;
        }

        public async Task<int[]> GetAvailableAgentIds(HashSet<int> agentIds)
        {
            return await _dbContext.Agent
                  .Where(x => agentIds.Contains(x.id) && x.status_flag.Equals(DomainConstant.StatusFlag.Enabled))
                  .Select(x => x.id).ToArrayAsync();
        }

        public async Task<bool> CheckExistedAsync(string agent_no)
        {
            return await _dbContext.Agent.AnyAsync(x =>
                x.agent_no.Equals(agent_no));
        }

        public async Task<string[]> CheckInvalidAgentNOs(List<Agent> agents)
        {
            var agentQuery = _dbContext.Agent.AsQueryable();

            var parameter = Expression.Parameter(typeof(Agent), "x");
            Expression predicate = Expression.Constant(false);

            foreach (var condition in agents)
            {
                var agentIdCondition = Expression.NotEqual(
                    Expression.Property(parameter, "id"),
                    Expression.Constant(condition.id)
                );

                var agentNoCondition = Expression.Equal(
                    Expression.Property(parameter, "agent_no"),
                    Expression.Constant(condition.agent_no)
                );

                var combinedCondition = Expression.AndAlso(agentIdCondition, agentNoCondition);
                predicate = Expression.OrElse(predicate, combinedCondition);
            }

            var lambda = Expression.Lambda<Func<Agent, bool>>(predicate, parameter);
            var agentNo = await agentQuery.Where(lambda)
                .Select(x => x.agent_no).ToArrayAsync();

            return agentNo;
        }

        public IQueryable<PagedAgentDetail> BuildFilterAgentQuery(PagedFilterAgentRequestModel request)
        {
            var agentQuery = _dbContext.Agent.AsNoTracking();

            // create_date_from
            if (request.create_date_from.HasValue)
            {
                agentQuery = agentQuery.Where(x => x.created_on >= request.create_date_from);
            }

            // create_date_to
            if (request.create_date_to.HasValue)
            {
                agentQuery = agentQuery.Where(x => x.created_on <= request.create_date_to);
            }

            // agent_type
            if (request.agent_type is not null && request.agent_type.Count > 0)
            {
                agentQuery = agentQuery.Where(x => request.agent_type.Contains(x.agent_type));
            }

            // status_flag
            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                agentQuery = agentQuery.Where(x => request.status_flag.Contains(x.status_flag));
            }

            var agentDetail = from agent in agentQuery
                              select new PagedAgentDetail
                              {
                                  id = agent.id,
                                  agent_no = agent.agent_no,
                                  agent_name = agent.agent_name,
                                  agent_type = agent.agent_type,
                                  status_flag = agent.status_flag,
                                  created_by = agent.created_by,
                                  created_on = agent.created_on,
                                  last_modified_by = agent.last_modified_by,
                                  last_modified_on = agent.last_modified_on
                              };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                agentDetail = agentDetail.Where(x =>
                    x.agent_name.Contains(request.Keyword) ||
                    x.agent_no.Contains(request.Keyword));
            }

            return agentDetail;
        }

        public async Task<List<Agent>> GetListAgentAsync(List<int> agentIds)
        {
            var agent = await _dbContext.Agent.Where(x => agentIds.Contains(x.id)).ToListAsync();
            return agent;
        }
    }
}

