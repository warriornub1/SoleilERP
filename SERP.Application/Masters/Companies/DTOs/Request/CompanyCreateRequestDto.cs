using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Masters.Companies.DTOs.Request
{
    public class CompanyCreateRequestDto
    {
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

        public class CompanyCreateRequestDtoValidator : AbstractValidator<CompanyCreateRequestDto>
        {
            public CompanyCreateRequestDtoValidator()
            {
                RuleFor(x => x.company_no).NotEmpty().NotNull();
                RuleFor(x => x).Must(x => x.company_no.Length <= 50)
                    .WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_no", 50));

                RuleFor(x => x.company_name).NotEmpty().NotNull();
                RuleFor(x => x).Must(x => x.company_name.Length <= 100)
                    .WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_name", 100));

                RuleFor(x => x.base_currency_id).NotEmpty().NotNull();
                RuleFor(x => x.registered_site_id).NotEmpty().NotNull();
                RuleFor(x => x.parent_group_id).NotEmpty().NotNull();

                RuleFor(x => x.company_registration_no).NotEmpty().NotNull();
                RuleFor(x => x).Must(x => x.company_name.Length <= 50)
                    .WithMessage(string.Format(ErrorMessages.LengthNotAccepted, "company_registration_no", 50));

                //RuleFor(x => x.gst_vat_registration_no).NotEmpty().NotNull();
                //RuleFor(x => x.gst_vat_registration_no.Length <= 50);

                //RuleFor(x => x.tax_registration_no).NotEmpty().NotNull();
                //RuleFor(x => x.tax_registration_no.Length <= 50);

                //RuleFor(x => x.intercompany_flag).NotEmpty().NotNull();
                //RuleFor(x => x.dormant_flag).NotEmpty().NotNull();

                RuleFor(x => x.status_flag).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.status_flag == DomainConstant.StatusFlag.Enabled ||
                    x.status_flag == DomainConstant.StatusFlag.Disabled)
                    .WithMessage(string.Format(ErrorMessages.CompanyFlagNotAccepted, "status_flag"));
            }
        }
    }
}
