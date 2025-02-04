using SERP.Application.Common.Dto;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Request
{
    public class SearchPackingInfoRequestDto : SearchPagedRequestDto
    {
        public int asn_header_id { get; set; }
    }

}
