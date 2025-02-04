using FluentValidation;
using SERP.Application.Finance.CostCenters.DTOs.Request;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Request
{
    public class DeleteNaturalAccountRequestModel
    {
        public List<DeleteNaturalAccountList> naturalAccountList { get; set; }
    }

    public class DeleteNaturalAccountList
    {
        public int id { get; set; }
    }

    public class DeleteNaturalAccountRequestModelValidator : AbstractValidator<DeleteNaturalAccountRequestModel>
    {
        public DeleteNaturalAccountRequestModelValidator()
        {
            RuleForEach(x => x.naturalAccountList)
                .SetValidator(new DeleteNaturalAccountListValidator());
        }
    }

    public class DeleteNaturalAccountListValidator : AbstractValidator<DeleteNaturalAccountList>
    {
        public DeleteNaturalAccountListValidator()
        {
            RuleFor(x => x.id).GreaterThan(0);

        }
    }
}
