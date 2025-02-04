using FluentValidation;
using Microsoft.AspNetCore.Http;
using SERP.Application.Common.Constants;
using SERP.Application.Transactions.FilesTracking.DTOs;
using SERP.Domain.Common.Constants;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Application.Transactions.Containers.DTOs.Request
{
    public class UpdateContainerByActionRequestDto
    {
        public int? container_id { get; set; }
        public string? container_no { get; set; }
        public string bay_no { get; set; }
        public string? remark { get; set; }
        public int? no_of_packages_unloaded { get; set; }
        public string? receiving_location { get; set; }
        public string action { get; set; }
        public List<IFormFile>? files { get; set; }
        public List<int>? DeleteFiles { get; set; }
        public class UpdateContainerByActionRequestDtoValidator : AbstractValidator<UpdateContainerByActionRequestDto>
        {
            public UpdateContainerByActionRequestDtoValidator()
            { 
                // Custom rule to ensure that either Id or ContainerNo has a value
                RuleFor(x => x)
                    .Must(x => !string.IsNullOrWhiteSpace(x.container_no) || (x.container_id != null && x.container_id.HasValue))
                    .WithMessage(ErrorMessages.EitherIdOrContainerNoMustHaveValue);
                //validate container_no length when container no is not empty
                RuleFor(x => x.container_no).MaximumLength(DomainDBLength.ContainerNo).When(x => !string.IsNullOrWhiteSpace(x.container_no));
                //validate bay no  when action = IN
                RuleFor(x => x.bay_no).NotEmpty().NotNull().MaximumLength(DomainDBLength.BayNo).When(x => x.action == ContainerUpdateAction.In);
                //Validate remark length when remark is not empty
                RuleFor(x => x.remark).MaximumLength(DomainDBLength.UnloadRemark).When(x => !string.IsNullOrWhiteSpace(x.remark));
                //validate no of packages unloaded when action = unload completed
                RuleFor(x => x.no_of_packages_unloaded).NotEmpty().NotNull().GreaterThanOrEqualTo(0).When(x=>x.action==ContainerUpdateAction.UploadCom);
                //validate action range
                RuleFor(x => x.action).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.action == ContainerUpdateAction.In ||
                    x.action == ContainerUpdateAction.UploadSt ||
                    x.action == ContainerUpdateAction.UploadCom ||
                    x.action == ContainerUpdateAction.Out )
                    .WithMessage(ErrorMessages.ActionNotSupport);
                //validate container id when action is not IN 
                RuleFor(x => x.container_id).NotEmpty().NotNull().When(x => x.action != ContainerUpdateAction.In);
            }
        }
    }
}
