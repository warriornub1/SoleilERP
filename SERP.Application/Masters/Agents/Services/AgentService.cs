using AutoMapper;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Agents.DTOs;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.Agents.DTOs.Response;
using SERP.Application.Masters.Agents.Interfaces;
using SERP.Application.Masters.Lovs.Interfaces;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Enums;
using SERP.Domain.Common.Model;
using SERP.Domain.Masters.Agents;
using SERP.Domain.Masters.Agents.Model;
using SERP.Domain.Masters.LOVs;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Agents.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly ILogger<AgentService> _logger;
        private readonly ILovRepository _lovRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AgentService(IAgentRepository agentRepository,
            ILogger<AgentService> logger,
            ILovRepository lovRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _agentRepository = agentRepository;
            _logger = logger;
            _lovRepository = lovRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AgentDto>> GetByAgentType(string agentType, bool onlyEnabled)
        {
            return _mapper.Map<List<AgentDto>>(await _agentRepository.GetByAgentType(agentType, onlyEnabled));
        }

        public async Task<List<int>> CreateAsync(string userId, List<CreateAgentRequestDto> requests)
        {
            var agentToInsert = new List<Agent>();
            foreach (var request in requests)
            {
                var agent = _mapper.Map<Agent>(request);
                agent.created_by = userId;
                agent.created_on = DateTime.Now;
                agentToInsert.Add(agent);
            }

            await ValidationRequest(agentToInsert);
            if (agentToInsert.Count > 0)
            {
                await _agentRepository.CreateRangeAsync(agentToInsert);
            }

            await _unitOfWork.SaveChangesAsync();
            return agentToInsert.Select(x => x.id).ToList();
        }

        public async Task UpdateAsync(string userId, List<UpdateAgentRequestDto> requests)
        {
            var existingAgent = await _agentRepository.GetListAgentAsync(requests.Select(x => x.id).ToList());
            var invalidAgentIds = requests.Select(x => x.id).Except(existingAgent.Select(x => x.id)).ToList();
            if (invalidAgentIds.Count > 0)
            {
                throw new NotFoundException(ErrorCodes.AgentNotFound, string.Format(ErrorMessages.AgentNotFound, nameof(Agent.id), string.Join(",", invalidAgentIds)));
            }

            var agentToUpdate = new List<Agent>();
            foreach (var request in requests)
            {
                var agent = existingAgent.Find(x => x.id == request.id);

                if (agent is null)
                {
                    _logger.LogError($"[UpdateAgent]: Agent with id {request.id} not found");
                    continue;
                }
                _mapper.Map(request, agent);
                agent.last_modified_by = userId;
                agent.last_modified_on = DateTime.Now;
                agentToUpdate.Add(agent);
            }

            await ValidationRequest(agentToUpdate);
            await _agentRepository.UpdateRangeAsync(agentToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(List<int> ids)
        {
            var existingAgent = await _agentRepository.GetListAgentAsync(ids);
            var invalidAgentIds = ids.Except(existingAgent.Select(x => x.id)).ToList();
            if (invalidAgentIds.Count > 0)
            {
                throw new NotFoundException(ErrorCodes.AgentNotFound, string.Format(ErrorMessages.AgentNotFound, nameof(Agent.id), string.Join(",", invalidAgentIds)));
            }

            await _agentRepository.DeleteRangeAsync(existingAgent);
            await _unitOfWork.SaveChangesAsync();
        }

        public PagedResponse<PagedAgentResponseDto> PagedFilterAgentAsync(SearchPagedRequestDto request, PagedFilterAgentRequestDto filters)
        {
            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            var query = _agentRepository.BuildFilterAgentQuery(new PagedFilterAgentRequestModel
            {
                Keyword = request.Keyword,
                create_date_from = filters.create_date_from,
                create_date_to = filters.create_date_to,
                agent_type = filters.agent_type,
                status_flag = filters.status_flag
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.Agent,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<PagedAgentDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return new PagedResponse<PagedAgentResponseDto>();
            }

            var totalPage = (int)Math.Ceiling(totalRows / (request.PageSize * 1.0));
            var pagedResponse = orderBy(query).Skip(skipRow).Take(pageable.Size).ToList();

            return new PagedResponse<PagedAgentResponseDto>
            {
                Items = _mapper.Map<List<PagedAgentResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }

        #region Private Methods
        private async Task ValidationRequest(List<Agent> agents)
        {
            var invalidAgentNos = await _agentRepository.CheckInvalidAgentNOs(agents);
            if (invalidAgentNos.Length > 0)
            {
                throw new NotFoundException(ErrorCodes.BranchPlantAlreadyExists, string.Format(ErrorMessages.AgentAlreadyExists, nameof(Agent.agent_no), string.Join(",", invalidAgentNos)));
            }

            var agentTypes = await GetLovList();
            var invalidAgentTypeIds = agents.Select(x => x.agent_type).Except(agentTypes.Select(x => x.lov_value)).ToList();
            if (invalidAgentTypeIds.Count > 0)
            {
                throw new BadRequestException(string.Format(ErrorMessages.AgentTypeInvalid, string.Join(",", invalidAgentTypeIds)));
            }
        }

        private async Task<List<Lov>> GetLovList()
        {
            var lov = await _lovRepository.GetByLovTypeAsync(
            [
                LOVs.Type.AgentTypes
            ], onlyEnabled: true);

            return lov.ToList();
        }
        #endregion
    }
}
