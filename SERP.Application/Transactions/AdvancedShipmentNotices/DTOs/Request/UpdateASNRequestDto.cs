using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class UpdateASNRequestDto
    {
        [AcceptValue(DomainConstant.Action.Submit,
            DomainConstant.Action.Draft)]
        public string action { get; set; }
        public List<UpdateASNInfoRequestDto> asns { get; set; }
    }

    public class UpdateASNInfoRequestDto
    {
        public UpdateASNHeaderRequestDto asn_header { get; set; }
        public List<UpdateASNDetailRequestDto> asn_details { get; set; }

        public List<int>? delete_detail_id { get; set; }
        //public UpdateShipmentInfoRequestDto? shipment_info { get; set; }
    }
}
