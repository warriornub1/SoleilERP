using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.CostCenters.DTOs.Request;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.CostCenters.DTOs.Validator
{
    public class BaseCostCenterValidator : AbstractValidator<CostCenterBaseDto>
    {
        public BaseCostCenterValidator()
        {
            RuleFor(x => x.cost_center_code).NotEmpty().NotNull();
            RuleFor(x => x).Must(x => x.cost_center_code.Length <= DomainDBLength.CostCenter.CostCenterCode);

            RuleFor(x => x.cost_center_description).NotEmpty().NotNull();
            RuleFor(x => x).Must(x => x.cost_center_description.Length <= DomainDBLength.CostCenter.CostCenterDescription);

            RuleFor(x => x.parent_group_id).NotEmpty().NotNull();
            RuleFor(x => x).Must(x => x.parent_group_id > 0);
            RuleFor(x => x).Must(x =>
                x.status_flag == DomainConstant.StatusFlag.Enabled ||
                x.status_flag == DomainConstant.StatusFlag.Disabled)
                .WithMessage(ErrorMessages.StatusFlagNotSupport);
        }
    }
}
