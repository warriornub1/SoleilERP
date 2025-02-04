
using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Finance.Groups.DTOs.Request
{
    public class UpdateGroupRequestModel
    {
        public int id { get; set; }
        public string group_code { get; set; }

        public string group_description { get; set; }

        public int level { get; set; }

        public int? parent_group_id { get; set; }

        public string status_flag { get; set; }

        public class UpdateGroupRequestModelValidator : AbstractValidator<UpdateGroupRequestModel>
        {
            public UpdateGroupRequestModelValidator()
            {
                RuleFor(x => x.id).NotEmpty().NotNull();
                RuleFor(x => x.group_code).NotEmpty().NotNull();
                RuleFor(x => x.group_description).NotEmpty().NotNull();

                RuleFor(x => x.level).NotEmpty().NotNull();

                RuleFor(x => x).Must(x =>
                    x.status_flag == DomainConstant.StatusFlag.Enabled ||
                    x.status_flag == DomainConstant.StatusFlag.Disabled)
                    .WithMessage(ErrorMessages.StatusFlagNotSupport);
            }
        }
    }
}
