using SERP.Domain.Transactions.Containers.Model;

namespace SERP.Domain.Transactions.AdvancedShipmentNotices.Model
{
    public class PageAsnResponseDetail
    {
        public int id { get; set; }
        public string asn_no { get; set; }
        public string status_flag { get; set; }
        public string branch_plant_no { get; set; }
        public string branch_plant_name { get; set; }
        public string ship_to_branch_plant_no { get; set; }
        public string ship_to_branch_plant_name { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        public DateOnly? estimated_putaway_date { get; set; }
        public DateOnly? received_date { get; set; }
        public string? internal_remarks { get; set; }
        public string? notes_to_cargo_team { get; set; }
        public bool attachment_flag { get; set; }
        public bool notes_to_warehouse_flag { get; set; }
        public List<ContainerDetail>? containers { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
