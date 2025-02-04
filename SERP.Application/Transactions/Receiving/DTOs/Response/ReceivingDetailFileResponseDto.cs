namespace SERP.Application.Transactions.Receiving.DTOs.Response
{
    public class ReceivingDetailFileResponseDto
    {
        public int receiving_header_id { get; set; }
        public string receiving_no { get;set; }
        public List<ReceivingDetailFileBinaryResponseDto> receiving_details { get; set; } = [];
    }
}
