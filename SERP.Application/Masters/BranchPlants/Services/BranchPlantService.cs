using AutoMapper;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Agents.DTOs.Request;
using SERP.Application.Masters.Agents.DTOs.Response;
using SERP.Application.Masters.Agents.Interfaces;
using SERP.Application.Masters.BranchPlants.DTOs;
using SERP.Application.Masters.BranchPlants.DTOs.Request;
using SERP.Application.Masters.BranchPlants.DTOs.Response;
using SERP.Application.Masters.BranchPlants.Interfaces;
using SERP.Application.Masters.Companies.Interfaces;
using SERP.Application.Masters.Sites.Interfaces;
using SERP.Domain.Common.Enums;
using SERP.Domain.Common.Model;
using SERP.Domain.Masters.Agents;
using SERP.Domain.Masters.Agents.Model;
using SERP.Domain.Masters.BranchPlants;
using SERP.Domain.Masters.BranchPlants.Models;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.BranchPlants.Services
{
    public class BranchPlantService : IBranchPlantService
    {
        private readonly IBranchPlantRepository _branchPlantRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<BranchPlantService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISiteRepository _siteRepository;

        public BranchPlantService(
            IBranchPlantRepository branchPlantRepository,
            ICompanyRepository companyRepository,
            ILogger<BranchPlantService> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ISiteRepository siteRepository)
        {
            _branchPlantRepository = branchPlantRepository;
            _companyRepository = companyRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _siteRepository = siteRepository;
        }
        public async Task<IEnumerable<BranchPlantGetByBUDto>> GetByCompanyAsync(string companyNo)
        {
            return _mapper.Map<List<BranchPlantGetByBUDto>>(await _branchPlantRepository.GetByCompany(companyNo));
        }

        public async Task <List<int>> CreateAsync(string userId, List<CreateBranchPlantRequestDto> request)
        {
           var IDlist = new List<int>();
            foreach (var req in request)
            {
                var validateRequest = _mapper.Map<ValidateBranchPlantRequest>(req);
                validateRequest.mode = SERPEnum.Mode.Insert;
                await ValidateCreateRequest(validateRequest);

                var branchPlant = _mapper.Map<BranchPlant>(req);
                branchPlant.created_by = userId;

                await _branchPlantRepository.CreateAsync(branchPlant);
                await _unitOfWork.SaveChangesAsync();
                IDlist.Add(branchPlant.id);
            }
            return IDlist;
        }

        public async Task UpdateAsync(string userId, List<UpdateBranchPlantRequestDto> request)
        {
            foreach (var req in request)
            {
                var branchPlant = await _branchPlantRepository.GetByIdAsync(x => x.id == req.id);
                if (branchPlant is null)
                {
                    throw new NotFoundException(ErrorCodes.BranchPlantNotFound, string.Format(ErrorMessages.BranchPlantNotFound, nameof(req.id), req.id));
                }
                var validateRequest = _mapper.Map<ValidateBranchPlantRequest>(req);
                validateRequest.mode = SERPEnum.Mode.Update;

                await ValidateCreateRequest(validateRequest);

                _mapper.Map(req, branchPlant);
                branchPlant.last_modified_by = userId;
                branchPlant.last_modified_on = DateTime.Now;

                await _branchPlantRepository.UpdateAsync(branchPlant);
                await _unitOfWork.SaveChangesAsync();
            }
          
        }

        public async Task DeleteAsync(int id)
        {
            var branchPlant = await _branchPlantRepository.GetByIdAsync(x => x.id == id);
            if (branchPlant is null)
            {
                throw new NotFoundException(ErrorCodes.BranchPlantNotFound, string.Format(ErrorMessages.BranchPlantNotFound, nameof(BranchPlant.id), id));
            }

            // TODO: Delete related data

            await _branchPlantRepository.DeleteAsync(branchPlant);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<BranchPlantGetByIdDto> GetById(int id)
        {
            return _mapper.Map<BranchPlantGetByIdDto>(await _branchPlantRepository.GetById(id));
        }

        #region Private Methods
        private async Task ValidateCreateRequest(ValidateBranchPlantRequest request)
        {
            var company = await _companyRepository.GetByIdAsync(x => x.id == request.company_id);
            if (company is null)
            {
                throw new NotFoundException(ErrorCodes.CompanyNotFound, string.Format(ErrorMessages.CompanyNotFound, nameof(request.company_id), request.company_id));
            }

            var site = await _siteRepository.GetByIdAsync(x => x.id == request.site_id);
            if (site is null)
            {
                throw new NotFoundException(ErrorCodes.SiteNotFound, string.Format(ErrorMessages.SiteNotFound, nameof(request.site_id), request.site_id));
            }

            if (request.mode == SERPEnum.Mode.Insert)
            {
                var isExistedBranchPlantNo = await _branchPlantRepository.CheckExistedBranchPlantNo(request.branch_plant_no);
                if (isExistedBranchPlantNo)
                {
                    throw new NotFoundException(ErrorCodes.BranchPlantAlreadyExists, string.Format(ErrorMessages.BranchPlantAlreadyExists, nameof(request.branch_plant_no), request.branch_plant_no));
                }
            }
        }

        public async Task DeleteAsync(List<int> ids)
        {
            var existingbranchPlant = await _branchPlantRepository.GetListBranchPlantAsync(ids);
            var invalidbranchPlantIds = ids.Except(existingbranchPlant.Select(x => x.id)).ToList();
            if (invalidbranchPlantIds.Count > 0)
            {
                throw new NotFoundException(ErrorCodes.BranchPlantNotFound, string.Format(ErrorMessages.BranchPlantNotFound, nameof(BranchPlant.id), string.Join(",", invalidbranchPlantIds)));
            }

            await _branchPlantRepository.DeleteRangeAsync(existingbranchPlant);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion
        public PagedResponse<PagedBranchPlantResponseDto> PagedFilterBranchPlantAsync(SearchPagedRequestDto request, PagedFilterBranchPlantRequestDto filters)
        {
            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            var query = _branchPlantRepository.BuildFilterBranchPlantQuery(new PagedFilterBranchPlantRequestModel
            {
                Keyword = request.Keyword,
                create_date_from = filters.create_date_from,
                create_date_to = filters.create_date_to,
                company_id = filters.company_id,
                status_flag = filters.status_flag
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.BranchPlant,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<PagedBranchPlantDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return new PagedResponse<PagedBranchPlantResponseDto>();
            }

            var totalPage = (int)Math.Ceiling(totalRows / (request.PageSize * 1.0));
            var pagedResponse = orderBy(query).Skip(skipRow).Take(pageable.Size).ToList();

            return new PagedResponse<PagedBranchPlantResponseDto>
            {
                Items = _mapper.Map<List<PagedBranchPlantResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }

       
    }
}
