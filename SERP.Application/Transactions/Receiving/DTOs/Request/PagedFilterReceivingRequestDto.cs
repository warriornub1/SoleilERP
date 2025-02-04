namespace SERP.Application.Transactions.Receiving.DTOs.Request
{
    public class PagedFilterReceivingRequestDto
    {
        public List<int>? branch_plant_id { get; set; }
        public string? inspected_by { get; set; }
        public List<int>? supplier_id { get; set; }
        public DateTime? inspection_due_date_from { get; set; }
        public DateTime? inspection_due_date_to { get; set; }
        public List<string>? status { get; set; }
        public DateTime? received_date_from { get; set; }
        public DateTime? received_date_to { get; set; }
        public Boolean? overdue_flag { get; set; }
    }
}
