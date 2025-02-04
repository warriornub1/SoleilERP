using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;

namespace SERP.Application.Masters.Lovs.DTOs.Request
{
    public class PagedFilterLovRequestDto : SearchPagedRequestModel
    {
        public List<string>? lovTypeList { get; set; }
        public List<string>? statusList { get; set; }
        public DateTime? create_date_from { get; set; }
        public DateTime? create_date_to { get; set; }
        public bool? default_flag { get; set; }

        public class PagedFilterLovRequestDtoValidator : AbstractValidator<PagedFilterLovRequestDto>
        {
            public PagedFilterLovRequestDtoValidator()
            {
                RuleFor(x => x.statusList)
                    .Must(statusList => statusList == null || statusList.All(status => status == DomainConstant.StatusFlag.Enabled || status == DomainConstant.StatusFlag.Disabled))
                    .WithMessage(ErrorMessages.StatusFlagNotSupport);

            }
        }
    }
}
