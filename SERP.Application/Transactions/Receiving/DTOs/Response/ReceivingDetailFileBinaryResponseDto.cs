namespace SERP.Application.Transactions.Receiving.DTOs.Response
{
    public class ReceivingDetailFileBinaryResponseDto : ReceivingDetailBaseResponseDto
    {
        public List<RcvFile> photos { get; set; } = [];
    }
    public class RcvFile
    {
        public int file_id { get; set; }
        public string file_type { get; set; }
        public byte[] photo { get; set; }
    }
}
