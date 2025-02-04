namespace SERP.Domain.Transactions.Receiving.Model
{
    public class ReceivingHeaderDetail
    {
        public int receiving_header_id { get; set; }
        public string receiving_no { get; set; }
        public string status_flag { get; set; }
        public string branch_plant { get; set; }
        public int? container_id { get; set; }
        public string? container_no { get; set; }
        public DateTime? received_on { get; set; }
        public DateTime? inspector_assigned_on { get; set; }
        public DateTime? inspection_start_on { get; set; }
        public DateTime? inspection_end_on { get; set; }
        public DateTime? inspection_due_date { get; set; }
        public string? inspected_by { get; set; }
        public bool lot_label_required_flag { get; set; }
        public int total_packages { get; set; }
        public int total_packages_unloaded { get; set; }
        public int no_of_items { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
        public int? asn_header_id { get; set; }
        public string? asn_no { get; set; }
    }
}
