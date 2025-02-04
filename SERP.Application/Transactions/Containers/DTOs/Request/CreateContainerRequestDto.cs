using FluentValidation;
using SERP.Application.Common.Constants;
using SERP.Domain.Common.Constants;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Application.Transactions.Containers.DTOs.Request
{
    public class CreateContainerRequestDto
    {
        public int inbound_shipment_blawb_id { get; set; }
        public string container_no { get; set; }
        public string status_flag { get; set; }
        public string? shipment_type { get; set; }
        public string? container_type { get; set; }
        public decimal? weight { get; set; }
        public class CreateContainerRequestDtoValidator : AbstractValidator<CreateContainerRequestDto>
        {
            public CreateContainerRequestDtoValidator()
            {
                RuleFor(x => x.inbound_shipment_blawb_id).NotEmpty().NotNull().When(x => x.status_flag == DomainConstant.Containers.StatusFlag.Incoming);
                RuleFor(x => x.container_no).NotEmpty().NotNull().MaximumLength(DomainDBLength.ContainerNo);
                RuleFor(x => x.status_flag).NotEmpty().NotNull().MaximumLength(DomainDBLength.StatusFlag);
                RuleFor(x => x).Must(x =>
                    x.status_flag == ContainerCreateStatusFlag.Incoming ||
                    x.status_flag == ContainerCreateStatusFlag.Unverified )
                    .WithMessage(ErrorMessages.StatusFlagNotSupport);
                RuleFor(x => x.shipment_type).MaximumLength(DomainDBLength.ShipmentType).When(x=>!string.IsNullOrWhiteSpace(x.shipment_type));
                RuleFor(x => x.weight).GreaterThan(0).When(x => x.weight!=null && x.weight.HasValue);
            }
        }
        public class CreateContainerRequestDtoListValidator : AbstractValidator<List<CreateContainerRequestDto>>
        {
            public CreateContainerRequestDtoListValidator()
            {
                // Use RuleForEach to validate each item in the Items list
                RuleForEach(x => x).SetValidator(new CreateContainerRequestDtoValidator());
            }
        }
    }
}
