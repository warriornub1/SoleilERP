namespace SERP.Domain.Masters.Items.Model
{
    public class ItemMappingDetail
    {
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string description_2 { get; set; }
        public string supplier_part_no { get; set; }
        public string supplier_material_code { get; set; }
        public string supplier_material_description { get; set; }
        public bool default_flag { get; set; }
        public string status_flag { get; set; }
        public DateTime created_on { get; set; } = DateTime.Now;
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
