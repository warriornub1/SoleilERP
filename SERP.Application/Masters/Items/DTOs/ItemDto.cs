namespace SERP.Application.Masters.Items.DTOs
{
    public class ItemDto
    {
        public int id { get; set; }
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string? description_2 { get; set; }
        public string? brand { get; set; }
        public string primary_uom { get; set; }
        public string secondary_uom { get; set; }
        public string purchasing_uom { get; set; }
        public int purchase_min_order_qty { get; set; }
        public int purchase_multiple_order_qty { get; set; }
        public ItemConversion item_conversion { get; set; }
        public bool label_required_flag { get; set; }
        public bool lot_control_flag { get; set; }
        public string inspection_instruction { get; set; }
        public string status_flag { get; set; }
        public DateTime created_on { get; set; } = DateTime.Now;
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class ItemConversion
    {
        public int primary_uom_qty { get; set; }
        public int secondary_uom_qty { get; set; }
    }
}
