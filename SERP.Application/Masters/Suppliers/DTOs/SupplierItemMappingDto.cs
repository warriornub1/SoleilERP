namespace SERP.Application.Masters.Suppliers.DTOs
{
    public class SupplierItemMappingDto
    {
        public int id { get; set; }
        public string supplier_material_code { get; set; }
        public string supplier_part_no { get; set; }
        public string supplier_material_description { get; set; }
        public bool default_flag { get; set; }
        public string status_flag { get; set; }
    }
}
