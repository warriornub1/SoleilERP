using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.Natural_Accounts.DTOs.Request;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Validator
{
    public class UpdateNaturalAccountRequestModelValidator : AbstractValidator<UpdateNaturalAccountRequestModel>
    {
        public UpdateNaturalAccountRequestModelValidator()
        {
            RuleFor(x => x.id > 0).NotEmpty().NotNull();
            Include(new BasicNaturalAccountValidator());
        }
    }
}
