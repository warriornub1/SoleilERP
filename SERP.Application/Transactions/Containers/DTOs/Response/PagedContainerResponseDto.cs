namespace SERP.Application.Transactions.Containers.DTOs.Response
{
    public class PagedContainerResponseDto
    {
        public int id { get; set; }
        public string container_no { get; set; }
        public string bay_no { get; set; }
        public DateTime? detention_date { get; set; }
        public int? no_of_packages { get; set; }
        public int? no_of_packages_unloaded { get; set; }
        public string status_flag { get; set; }
        public string shipment_type { get; set; }
        public string container_type { get; set; }
        public decimal? weight { get; set; }
        public DateTime? unload_start_on { get; set; }
        public DateTime? unload_end_on { get; set; }
        public string unloaded_by { get; set; }
        public string unload_remark { get; set; }
        public DateTime? received_on { get; set; }
        public string received_by { get; set; }
        public DateTime? released_on { get; set; }
        public string released_by { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
