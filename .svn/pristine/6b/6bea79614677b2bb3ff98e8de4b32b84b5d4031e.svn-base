using FluentValidation;
using Microsoft.AspNetCore.Http;
using SERP.Application.Common.Constants;
using static SERP.Application.Transactions.Receiving.DTOs.Request.CreateReceivingDetailRequestDto;

namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class CreateReceivingRequestDto : ReceivingHeaderRequestDto
    {
        public int supplier_id { get; set; }
        public int branch_plant_id { get; set; }
        //public string received_from_document_type { get; set; }
        public int? asn_header_id { get; set; }
        public List<CreateReceivingDetailRequestDto> receiving_details { get; set; }

        //public List<ReceivingAttachment>? files { get; set; }

        public class CreateReceivingRequestDtoValidator : AbstractValidator<CreateReceivingRequestDto>
        {
            public CreateReceivingRequestDtoValidator()
            {
                RuleFor(x => x.branch_plant_id).NotEmpty().NotNull();
                RuleFor(x => x.supplier_id).NotEmpty().NotNull();
                RuleFor(x => x.draft_flag).NotNull();
                //RuleFor(x => x.received_from_document_type).NotEmpty().NotNull();
                //RuleFor(x => x).Must(x =>
                //    x.received_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder ||
                //    x.received_from_document_type == ApplicationConstant.ReceivedFromDocType.AdvancedShipmentNotice)
                //    .WithMessage(ErrorMessages.ReceivedFromDocTypeNotSupport);
                RuleFor(x => x.receiving_details).NotNull().NotEmpty();
                // Use RuleForEach to validate each item in the Items list
                RuleForEach(x => x.receiving_details).SetValidator(x=>new CreateReceivingDetailRequestDtoValidator());

            }
        }
    }
    public class CreateReceivingDetailRequestDto : ReceivingDetailRequestDto
    {
        public class CreateReceivingDetailRequestDtoValidator : AbstractValidator<CreateReceivingDetailRequestDto>
        {
            public CreateReceivingDetailRequestDtoValidator()
            {
                RuleFor(x => x.po_detail_id).NotEmpty().NotNull();
                RuleFor(x => x.qty).NotEmpty().NotNull();
            }
        }
    }

    //public class ReceivingAttachment
    //{
    //    public IFormFile file { get; set; }
    //    public string doc_type { get; set; }
    //}
}
