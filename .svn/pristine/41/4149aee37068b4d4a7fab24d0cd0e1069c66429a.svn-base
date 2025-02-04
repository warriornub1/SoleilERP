using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.Groups.DTOs.Request
{
    public class CreateGroupRequestModel
    {
        public string group_code { get; set; }

        public string group_description { get; set; }

        public string group_type { get; set; }

        public int level { get; set; }

        public int? parent_group_id { get; set; }

        public string status_flag { get; set; }

        public class CreateGroupRequestModelValidator : AbstractValidator<CreateGroupRequestModel>
        {
            public CreateGroupRequestModelValidator()
            {
                RuleFor(x => x.group_code).NotEmpty().NotNull();
                RuleFor(x => x.group_description).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.group_type == DomainConstant.Group.GroupType.Company ||
                    x.group_type == DomainConstant.Group.GroupType.NaturalAccount ||
                    x.group_type == DomainConstant.Group.GroupType.CostCenter ||
                    x.group_type == DomainConstant.Group.GroupType.RevenueCenter)
                    .WithMessage(ErrorMessages.GroupTypeNotSupported);

                RuleFor(x => x.level).NotEmpty().NotNull();
                RuleFor(x => x.status_flag).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.status_flag == DomainConstant.StatusFlag.Enabled ||
                    x.status_flag == DomainConstant.StatusFlag.Disabled)
                    .WithMessage(ErrorMessages.StatusFlagNotSupport);

            }
        }
    }
}
