using SERP.Application.Masters.Countries.DTOs.Response;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.DTOs.Response
{
    public class PackingDiscrepancyListResponseDto
    {
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string supplier_part_no { get; set; }
        public int asn_qty { get; set; }
        public int packing_list_qty { get; set; }
        public string uom { get; set; }
        public CountryBasicResponseDto? country_of_origin { get; set; }
    }
}
