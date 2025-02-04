namespace SERP.Domain.Transactions.Receiving.Model
{
    public class ReceivingItemModel 
    {
        public int receiving_header_id { get; set; }
        public string package_no { get; set; }
        public string document_no { get; set; }
        public int item_id { get; set; }
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string supplier_part_no { get; set; }
        public string inspection_instruction { get; set; }
        public Item_Uom_Conversion? item_uom_conversion { get; set; }
        public string primary_uom { get; set; }
        public string secondary_uom { get; set; }
        public int package_qty { get; set; }
        public int document_qty { get; set; }
        public int receiving_qty { get; set; }
    }

    public class Item_Uom_Conversion
    {
        public int primary_uom_qty { get; set; }
        public int secondary_uom_qty { get; set; }
    }
}
