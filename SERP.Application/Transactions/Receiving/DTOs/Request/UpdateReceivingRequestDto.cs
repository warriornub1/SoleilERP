using FluentValidation;
using static SERP.Application.Transactions.Receiving.DTOs.Request.UpdateReceivingDetailRequestDto;

namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class UpdateReceivingRequestDto : ReceivingHeaderRequestDto
    {
        public int receiving_header_id { get; set; }
        public List<UpdateReceivingDetailRequestDto> receiving_details { get; set; }
        public class UpdateReceivingRequestDtoValidator : AbstractValidator<UpdateReceivingRequestDto>
        {
            public UpdateReceivingRequestDtoValidator()
            {
                RuleFor(x => x.receiving_header_id).NotEmpty().NotNull();
                RuleFor(x => x.draft_flag).NotNull();
                RuleFor(x => x.receiving_details).NotNull().NotEmpty();
                // Use RuleForEach to validate each item in the Items list
                RuleForEach(x => x.receiving_details).SetValidator(x => new UpdateReceivingDetailRequestDtoValidator());
            }
        }

    }
    public class UpdateReceivingDetailRequestDto : ReceivingDetailRequestDto
    {
        public int? receiving_detail_id { get; set; }
        public class UpdateReceivingDetailRequestDtoValidator : AbstractValidator<UpdateReceivingDetailRequestDto>
        {
            public UpdateReceivingDetailRequestDtoValidator()
            {
                RuleFor(x => x.receiving_detail_id).NotEmpty().NotNull().When(x => x.po_detail_id == null); 
                RuleFor(x => x.qty).NotEmpty().NotNull();
            }
        }
    }
}
