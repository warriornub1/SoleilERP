namespace SERP.Application.Transactions.Receiving.DTOs.Response
{
    public class ReceivingDetailFileUrlResponseDto
    {
        public int receiving_header_id { get; set; }
        public string receiving_no { get;set; }
        public List<ReceivingDetailBaseResponseDto> receiving_details { get; set; } = [];
        public List<RcvFileDetail> photos { get; set; } = [];  
    }
    public class RcvFileDetail
    {
        public int id { get; set; }
        public int receiving_detail_id { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }
        public string url_path { get; set; }
    }
}
