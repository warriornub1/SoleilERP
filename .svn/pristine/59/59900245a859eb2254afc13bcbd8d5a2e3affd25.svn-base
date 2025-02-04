using FluentValidation;

namespace SERP.Application.Finance.CostCenters.DTOs.Request
{
    public class DeleteCostCenterRequestDto
    {
        public List<DeleteCostCenter> costCenterList { get; set; }
    }

    public class DeleteCostCenter
    {
        public int id { get; set; }
    }

    public class DeleteCostCenterRequestModelValidator : AbstractValidator<DeleteCostCenterRequestDto>
    {
        public DeleteCostCenterRequestModelValidator()
        {
            RuleForEach(x => x.costCenterList)
                .SetValidator(new DeleteCostCenterListValidator());
        }
    }

    public class DeleteCostCenterListValidator : AbstractValidator<DeleteCostCenter>
    {
        public DeleteCostCenterListValidator()
        {
            RuleFor(x => x.id).GreaterThan(0);

        }
    }
}
