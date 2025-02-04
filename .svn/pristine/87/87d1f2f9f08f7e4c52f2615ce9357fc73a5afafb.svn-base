using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Masters.Lovs.DTOs.Request
{
    public class CreateLovRequestDto
    {
        public string lov_type { get; set; }
        public string lov_value { get; set; }
        public string lov_label { get; set; }
        public string? extended_data_1 { get; set; }
        public string? extended_data_2 { get; set; }
        public string? description { get; set; }
        public bool default_flag { get; set; }
        public string status_flag { get; set; }

        public class CreateLovRequestDtoValidator : AbstractValidator<CreateLovRequestDto>
        {
            public CreateLovRequestDtoValidator()
            {
                RuleFor(x => x.lov_type).NotEmpty().NotNull();
                RuleFor(x => x.lov_value).NotEmpty().NotNull();
                RuleFor(x => x.lov_label).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.status_flag == DomainConstant.StatusFlag.Enabled ||
                    x.status_flag == DomainConstant.StatusFlag.Disabled)
                    .WithMessage(ErrorMessages.StatusFlagNotSupport);
            }
        }

    }
}
