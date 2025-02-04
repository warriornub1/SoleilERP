using FluentValidation;
using SERP.Application.Finance.CompanyStructures.Interface;
using SERP.Application.Finance.Groups.Interfaces;
using SERP.Application.Finance.RevenueCenters.DTOs.Request;
using SERP.Application.Finance.RevenueCenters.Interface;

namespace SERP.Application.Finance.RevenueCenters.DTOs.Validator
{
    public class CreateRevenueCenterModelValidator : AbstractValidator<CreateRevenueCenterRequestModel>
    {
        private readonly ICompanyStructureRepository _companyStructureRepository;
        private readonly IRevenueCenterRepository _revenueCenterRepository;
        private readonly IGroupRepository _groupRepository;

        public CreateRevenueCenterModelValidator(ICompanyStructureRepository companyStructureRepository, IRevenueCenterRepository revenueCenterRepository, IGroupRepository groupRepository)
        {
            _companyStructureRepository = companyStructureRepository;
            _revenueCenterRepository = revenueCenterRepository;
            _groupRepository = groupRepository;

            Include(new IRevenueCenterDtoValidator(_companyStructureRepository, _revenueCenterRepository, _groupRepository));
            Include(new BaseRevenueCenterValidator());
        }
    }
}
