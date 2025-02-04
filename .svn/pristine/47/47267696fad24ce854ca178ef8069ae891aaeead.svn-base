using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.Natural_Accounts.DTOs.Request;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Validator
{
    public class BasicNaturalAccountValidator : AbstractValidator<NaturalAccountBaseDto>
    {
        public BasicNaturalAccountValidator()
        {
            RuleFor(x => x.natural_account_code).NotEmpty().NotNull();
            RuleFor(x => x).Must(x => x.natural_account_code.Length <= DomainDBLength.NaturalAccountDBLength.NaturalAccountCode);

            RuleFor(x => x.natural_account_description).NotEmpty().NotNull();
            RuleFor(x => x).Must(x => x.natural_account_description.Length <= DomainDBLength.NaturalAccountDBLength.NaturalAccountDescription);

            RuleFor(x => x).Must(x =>
               x.natural_account_type == DomainConstant.NaturalAccount.NaturalAccountType.Asset ||
               x.natural_account_type == DomainConstant.NaturalAccount.NaturalAccountType.Liability ||
               x.natural_account_type == DomainConstant.NaturalAccount.NaturalAccountType.Shareholders_Equity ||
               x.natural_account_type == DomainConstant.NaturalAccount.NaturalAccountType.PAndL)
                .WithMessage(ErrorMessages.NaturalAccountTypeNotAccepted);

            RuleFor(x => x.status_flag).NotEmpty().NotNull();
            RuleFor(x => x).Must(x =>
                x.status_flag == DomainConstant.StatusFlag.Enabled ||
                x.status_flag == DomainConstant.StatusFlag.Disabled)
                .WithMessage(ErrorMessages.StatusFlagNotSupport);
        }
    }
}
