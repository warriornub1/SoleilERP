namespace SERP.Domain.Transactions.Containers.Model
{
    public class ContainerListForAsnResponseDetail
    {
        public int container_id { get; set; }
        public string container_no { get; set; }
        public string status_flag { get; set; }
        public string shipment_type { get; set; }
        public DateTime? detention_date { get; set; }
        public decimal? weight { get; set; }
        public string? seal_no { get; set; }
    }
}
