using FluentValidation;

namespace SERP.Application.Finance.Groups.DTOs.Request
{
    public class DeleteGroupRequestModel
    {
        public List<DeleteGroupId> groupList { get; set; }
    }

    public class DeleteGroupId
    {
        public int id { get; set; }
    }

    public class DeleteGroupRequestModelValidator : AbstractValidator<DeleteGroupRequestModel>
    {
        public DeleteGroupRequestModelValidator()
        {
            RuleFor(x => x.groupList)
                .NotNull().WithMessage("Group list cannot be null.")
                .NotEmpty().WithMessage("Group list cannot be empty.");

            RuleForEach(x => x.groupList)
                .SetValidator(new DeleteGroupIdValidator());
        }
    }

    public class DeleteGroupIdValidator : AbstractValidator<DeleteGroupId>
    {
        public DeleteGroupIdValidator()
        {
            RuleFor(x => x.id).NotEmpty().NotNull();
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Group ID must be greater than zero.");
        }
    }
}
