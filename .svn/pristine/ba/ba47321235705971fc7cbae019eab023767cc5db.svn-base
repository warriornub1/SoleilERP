using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Application.Finance.RevenueCenters.DTOs.Request;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.RevenueCenters.DTOs.Validator
{
    public class UpdateRevenueCenterModelValidator : AbstractValidator<UpdateRevenueCenterRequestModel>
    {
        public UpdateRevenueCenterModelValidator()
        {
            RuleFor(x => x.id > 0).NotEmpty().NotNull();
            Include(new BaseRevenueCenterValidator());
        }
    }
}
