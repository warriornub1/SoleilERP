namespace SERP.Domain.Transactions.Receiving.Model
{
    public class ReceivingFileModel
    {
        public int id { get; set; }
        public int receiving_detail_id { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }
        public string url_path { get; set; }
    }
}
