using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Masters.Companies.DTOs.Request
{
    public class UpdateCompanyRequestDto
    {
        public int id { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set; }
        public int base_currency_id { get; set; }
        public int registered_site_id { get; set; }
        public int parent_group_id { get; set; }
        public string company_registration_no { get; set; }
        public string? gst_vat_registration_no { get; set; }
        public string? tax_registration_no { get; set; }
        public bool intercompany_flag { get; set; }
        public bool dormant_flag { get; set; }
        public string status_flag { get; set; }

        public class UpdateCompanyRequestDtoValidator : AbstractValidator<UpdateCompanyRequestDto>
        {
            public UpdateCompanyRequestDtoValidator()
            {
                RuleFor(x => x.id)
                    .NotEmpty().NotNull()
                    .GreaterThan(0).WithMessage(string.Format(ErrorMessages.IdGreaterThanZero, "id"));

                RuleFor(x => x.company_no)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "company_no"))
                    .MaximumLength(50).WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_no", 50));

                RuleFor(x => x.company_name)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "company_name"))
                    .MaximumLength(100).WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_name", 100));

                RuleFor(x => x.base_currency_id)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "base_currency_id"))
                    .GreaterThan(0).WithMessage(string.Format(ErrorMessages.IdGreaterThanZero, "base_currency_id"));

                RuleFor(x => x.registered_site_id)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "registered_site_id"))
                    .GreaterThan(0).WithMessage(string.Format(ErrorMessages.IdGreaterThanZero, "registered_site_id"));

                RuleFor(x => x.parent_group_id)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "parent_group_id"))
                    .GreaterThan(0).WithMessage(string.Format(ErrorMessages.IdGreaterThanZero, "parent_group_id"));

                RuleFor(x => x.company_registration_no)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "company_registration_no"))
                    .MaximumLength(50).WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_registration_no", 50));

                RuleFor(x => x.gst_vat_registration_no)
                    .MaximumLength(50).WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_registration_no", 50));

                RuleFor(x => x.tax_registration_no)
                    .MaximumLength(50).WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "tax_registration_no", 50));

                RuleFor(x => x.status_flag)
                    .NotEmpty().WithMessage(string.Format(ErrorMessages.IsRequired, "status_flag"))
                    .Must(x => x == DomainConstant.StatusFlag.Enabled ||
                    x == DomainConstant.StatusFlag.Disabled)
                    .WithMessage(string.Format(ErrorMessages.CompanyFlagNotAccepted, "status_flag"));

                //RuleFor(x => x.intercompany_flag).NotEmpty().NotNull();
                //RuleFor(x => x.dormant_flag).NotEmpty().NotNull();
            }
        }
    }
}
