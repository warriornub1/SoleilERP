namespace SERP.Application.Transactions.Receiving.DTOs.Response
{
    public class ReceivingLineNoResponseDto
    {
        public List<ReceivingLineDetailDTO> receiving_line_nos { get; set; } = [];
    }
    public class ReceivingLineDetailDTO
    { 
        public int receiving_detail_id { get; set; }
        public int receiving_detail_line_no { get; set; }
    }
}
