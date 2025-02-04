using FluentValidation;
using SERP.Application.Finance.CompanyStructures.Interface;
using SERP.Application.Finance.CostCenters.DTOs.Request;
using SERP.Application.Finance.CostCenters.Interface;
using SERP.Application.Finance.Groups.Interfaces;

namespace SERP.Application.Finance.CostCenters.DTOs.Validator
{
    public class CreateCostCenterResponseModelValidator : AbstractValidator<CreateCostCenterRequestModel>
    {
        private readonly ICompanyStructureRepository _companyStructureRepository;
        private readonly ICostCenterRepository _costCenterRepository;
        private readonly IGroupRepository _groupRepository;

        public CreateCostCenterResponseModelValidator(ICompanyStructureRepository companyStructureRepository, ICostCenterRepository costCenterRepository, IGroupRepository groupRepository)
        {
            _companyStructureRepository = companyStructureRepository;
            _costCenterRepository = costCenterRepository;
            _groupRepository = groupRepository;

            Include(new ICostCenterDtoValidator(_companyStructureRepository, _costCenterRepository, _groupRepository));
        }
    }
}
