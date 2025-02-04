using SERP.Domain.Common;

namespace SERP.Domain.Transactions.Receiving
{
    public class ReceivingFile : BaseModel
    {
        public int receiving_header_id { get; set; }
        public int receiving_detail_id { get; set; }
        public int file_id { get; set; }
    }
}
