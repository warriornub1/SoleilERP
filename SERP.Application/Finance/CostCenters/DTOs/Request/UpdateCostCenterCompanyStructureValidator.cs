using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.CostCenters.DTOs.Request
{
    public class UpdateCostCenterCompanyStructureValidator : AbstractValidator<UpdateCostCenterCompanyStructureModel>
    {
        public UpdateCostCenterCompanyStructureValidator()
        {
            RuleFor(x => x.cost_center_id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.company_structure_id)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .When(x => x.action_flag == DomainConstant.ActionFlag.Create);

            RuleFor(x => x.company_structure_id)
                .Empty()
                .When(x => x.action_flag == DomainConstant.ActionFlag.Delete);


            RuleFor(x => x).Must(x =>
                x.action_flag == DomainConstant.ActionFlag.Create ||
                x.action_flag == DomainConstant.ActionFlag.Delete)
                        .WithMessage(ErrorMessages.ActionNotSupport);
        }
    }
}
