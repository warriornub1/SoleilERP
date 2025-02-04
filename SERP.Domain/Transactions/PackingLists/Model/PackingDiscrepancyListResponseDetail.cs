using SERP.Domain.Masters.Countries.Models;

namespace SERP.Domain.Transactions.PackingLists.Model
{
    public class PackingDiscrepancyListResponseDetail
    {
        public int id { get; set; }
        public string item_no { get; set; }
        public string description_1 { get; set; }
        public string supplier_part_no { get; set; }
        public int asn_qty { get; set; }
        public int packing_list_qty { get; set; }
        public string uom { get; set; }
        public CountryBasicDetail? country_of_origin { get; set; }
    }
}
