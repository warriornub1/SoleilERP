using FluentValidation;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.Containers.DTOs.Request
{
    public class UpdateContainerRequestDto
    {
        public int container_id { get; set; }
        public int? inbound_shipment_blawb_id { get; set; }
        public string? container_no { get; set; }
        public string? shipment_type { get; set; }
        public string? container_type { get; set; }
        public decimal? weight { get; set; }
        public class UpdateContainerRequestDtoValidator : AbstractValidator<UpdateContainerRequestDto>
        {
            public UpdateContainerRequestDtoValidator()
            {
                RuleFor(x => x.inbound_shipment_blawb_id).NotEmpty();
                RuleFor(x => x.container_no).MaximumLength(DomainDBLength.ContainerNo).When(x => !string.IsNullOrWhiteSpace(x.container_no));
                RuleFor(x => x.shipment_type).MaximumLength(DomainDBLength.ShipmentType).When(x => !string.IsNullOrWhiteSpace(x.shipment_type));
                RuleFor(x => x.weight).GreaterThan(0).When(x => x.weight != null && x.weight.HasValue);
            }
        }
        public class UpdateContainerRequestDtoListValidator : AbstractValidator<List<UpdateContainerRequestDto>>
        {
            public UpdateContainerRequestDtoListValidator()
            {
                // Use RuleForEach to validate each item in the Items list
                RuleForEach(x => x).SetValidator(new UpdateContainerRequestDtoValidator());
            }
        }
    }
}
