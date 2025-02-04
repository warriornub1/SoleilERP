using FluentValidation;
using SERP.Application.Common.Constants;
using static SERP.Application.Common.Constants.ApplicationConstant;

namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class UpdateReceivingByActionDto 
    {
        public int receiving_header_id { get; set; }
        public string action { get; set; }
        public string? inspected_by { get; set; }
        public class UpdateReceivingByActionDtoValidator : AbstractValidator<UpdateReceivingByActionDto>
        {
            public UpdateReceivingByActionDtoValidator()
            {
                RuleFor(x => x.receiving_header_id).NotEmpty().NotNull();
                RuleFor(x => x).Must(x =>
                    x.action == ReceivingUpdateAction.AssignInspector ||
                    x.action == ReceivingUpdateAction.UnassignInspector ||
                    x.action == ReceivingUpdateAction.InspectionStart)
                    .WithMessage(ErrorMessages.ActionNotSupport);
                RuleFor(x => x.inspected_by).NotEmpty().NotNull().When(x=>x.action==ReceivingUpdateAction.AssignInspector);
            }
        }

    }
}
