using FluentValidation;
using SERP.Application.Common.Constants;

namespace SERP.Application.Masters.Companies.DTOs.Request
{
    public class DeleteCompanyRequestsDto
    {
        public List<Identity> companyList { get; set; }

        public class DeleteCompanyRequestsValidator : AbstractValidator<DeleteCompanyRequestsDto>
        {
            public DeleteCompanyRequestsValidator()
            {
                RuleFor(req => req.companyList)
                    .NotNull().NotEmpty();

                RuleForEach(req => req.companyList)
                    .SetValidator(new IdentityValidator());
            }
        }
        public class IdentityValidator : AbstractValidator<Identity>
        {
            public IdentityValidator()
            {
                RuleFor(identity => identity.Id)
                    .GreaterThan(0)
                    .WithMessage(string.Format(ErrorMessages.IdGreaterThanZero, "Id"));
            }
        }
    }

    public class Identity
    {
        public int Id { get; set; }
    }

}
