using FluentValidation;
using SERP.Application.Finance.CostCenters.DTOs.Request;

namespace SERP.Application.Finance.CostCenters.DTOs.Validator
{
    public class UpdateCostCenterModelValidator : AbstractValidator<UpdateCostCenterRequestModel>
    {

        public UpdateCostCenterModelValidator()
        {

            RuleFor(x => x.id > 0).NotEmpty().NotNull();
            Include(new BaseCostCenterValidator());
        }
    }
}
