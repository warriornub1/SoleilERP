using AutoMapper;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Finance.CompanyStructures.DTOs.Request;
using SERP.Application.Finance.CompanyStructures.DTOs.Response;
using SERP.Application.Finance.CompanyStructures.Interface;
using SERP.Application.Finance.CostCenters.Interface;
using SERP.Application.Finance.EmployeeStructureMappings.Interface;
using SERP.Application.Finance.RevenueCenters.Interface;
using SERP.Application.Masters.Companies.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;

namespace SERP.Application.Finance.CompanyStructures.Services
{
    public class CompanyStructureService : ICompanyStructureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyStructureRepository _companyStructureRepository;
        private readonly ICostCenterRepository _costCenterRepository;
        private readonly IRevenueCenterRepository _revenueCenterRepository;
        private readonly IEmployeeStructureMappingRepository _employeeStructureMappingRepository;

        public CompanyStructureService(IUnitOfWork unitOfWork, IMapper mapper, ICompanyRepository companyRepository, ICompanyStructureRepository companyStructureRepository,
                ICostCenterRepository costCenterRepository, IRevenueCenterRepository revenueCenterRepository, IEmployeeStructureMappingRepository employeeStructureMappingRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _companyStructureRepository = companyStructureRepository;
            _costCenterRepository = costCenterRepository;
            _revenueCenterRepository = revenueCenterRepository;
            _employeeStructureMappingRepository = employeeStructureMappingRepository;
        }

        public async Task<PagedResponse<SearchCompanyStructureResponseModel>> SearchPagedAsync(int page, int pageSize, string keyword, string sortBy, bool sortAscending, SearchCompanyStructureRequestModel request)
        {

            // add fluent validation

            var sortedCompanyStructures = await _companyStructureRepository.GetCompanyStructSearchPage(keyword, sortBy, sortAscending, request.company_id, request.org_type, request.status_flag,
                                                                        request.in_charge_employee_id, request.department, request.divison, request.section);
            int totalRows = sortedCompanyStructures.Count();
            if (totalRows == 0)
                return new PagedResponse<SearchCompanyStructureResponseModel>();

            var pageable = PagingUtilities.GetPageable(page, pageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);
            var pagedResponse = sortedCompanyStructures.Skip(skipRow).Take(pageable.Size).ToList();

            return new PagedResponse<SearchCompanyStructureResponseModel>
            {
                Items = pagedResponse,
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size,
            };
        }

        public async Task<IEnumerable<CompanyStructureResponseModel>> GetAllLimitedAsync(int company_id, int? org_type, bool onlyEnabled)
        {
            try
            {
                var companyStuctures = await _companyStructureRepository.GeCompanyStructureLimited(company_id, org_type, onlyEnabled);

                if (companyStuctures.Count() == 0)
                    throw new BadRequestException(ErrorCodes.CompanyStructNotFound, ErrorMessages.CompanyStructNotFound);

                List<CompanyStructureResponseModel> response = _mapper.Map<List<CompanyStructureResponseModel>>(companyStuctures);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync(DeleteCompanyStructureRequest request)
        {
            try
            {
                var requestIds = request.id.ToList();

                // id exist in CompanyStructure table
                var companyStructures = await _companyStructureRepository.FindMultiple(x => requestIds.Contains(x.id));

                var IdsNotFound = requestIds.Except(companyStructures.Select(x => x.id)).ToList();
                if (IdsNotFound.Count() > 0)
                    throw new BadRequestException(ErrorCodes.RequestIdNotFound, string.Format( ErrorMessages.RequestIdNotFound, string.Join(", ", IdsNotFound)));

                // CompanyStructure record status_flag is D: Disabled
                var disabledCompanyStructures = companyStructures.Where(x => x.status_flag == DomainConstant.StatusFlag.Disabled ).ToList();
                if(disabledCompanyStructures.Count() > 0)
                    throw new BadRequestException(ErrorCodes.CompanyStructDisabled, string.Format(ErrorMessages.CompanyStructDisabled, string.Join(", ", disabledCompanyStructures.Select(x => x.org_no))));

                // CompanyStructure.id is not used in CompanyStructure.parent_id
                var companyStructuresParent = await _companyStructureRepository.FindMultiple(x => requestIds.Contains(x.parent_id.Value));
                if(companyStructuresParent.Count() > 0)
                    throw new BadRequestException(ErrorCodes.CompanyStructParentInUse, string.Format(ErrorMessages.CompanyStructParentInUse, string.Join(", ", companyStructuresParent.Select(x => x.parent_id))));

                // CompanyStructure.id does not exist in CostCenter and RevenueCenter table
                var costCenters = await _costCenterRepository.FindMultiple(x => requestIds.Contains(x.company_structure_id.Value));
                var revenueCenters = await _revenueCenterRepository.FindMultiple(x => requestIds.Contains(x.company_structure_id.Value));

                if (costCenters.Count() > 0)
                    throw new BadRequestException(ErrorCodes.CostCenterContainsCompanyStructureID, string.Format(ErrorMessages.CostCenterContainsCompanyStructureID, string.Join(", ", costCenters.Select(x => x.cost_center_code))));

                if (revenueCenters.Count() > 0)
                    throw new BadRequestException(ErrorCodes.RevenueCenterContainsCompanyStructureID, string.Format(ErrorMessages.RevenueCenterContainsCompanyStructureID, string.Join(", ", revenueCenters.Select(x => x.revenue_center_code))));


                // CompanyStructure.id does not exist in EmployeeStructureMapping table
                var employeeStructureMapping = await _employeeStructureMappingRepository.FindMultiple(x => requestIds.Contains(x.company_structure_id));
                if (employeeStructureMapping.Count() > 0)
                    throw new BadRequestException(ErrorCodes.EmployeeCompanyStructureMappingContainsCompanyStructureID, string.Format(ErrorMessages.EmployeeCompanyStructureMappingContainsCompanyStructureID, string.Join(", ", employeeStructureMapping.Select(x => x.company_structure_id))));

                await _companyStructureRepository.DeleteRangeAsync(companyStructures);
                await _unitOfWork.SaveChangesAsync();

            }
            catch
            {
                throw;
            }
        }
    }
}
