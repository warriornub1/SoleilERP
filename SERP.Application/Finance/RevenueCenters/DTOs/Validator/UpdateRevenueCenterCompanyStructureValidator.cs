using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.RevenueCenters.DTOs.Request;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.RevenueCenters.DTOs.Validator
{
    public class UpdateRevenueCenterCompanyStructureValidator : AbstractValidator<UpdateRevenueCenterCompanyStructureModel>
    {
        public UpdateRevenueCenterCompanyStructureValidator()
        {
            RuleFor(x => x.revenue_center_id)
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
