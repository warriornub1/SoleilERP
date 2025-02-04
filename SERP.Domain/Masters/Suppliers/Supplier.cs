using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Suppliers
{
    public class Supplier : BaseModel
    {
        public int group_supplier_id { get; set; }

        public int registered_site_id { get; set; }

        [StringLength(50)]
        public string supplier_no { get; set; }

        [StringLength(1)]
        public string status_flag { get; set; }

        [StringLength(100)]
        public string supplier_name { get; set; }

        public bool service_flag { get; set; }

        public bool product_flag { get; set; }

        public bool authorised_flag { get; set; }

        [StringLength(50)]
        public string? payment_term { get; set; }

        [StringLength(50)]
        public string? payment_method { get; set; }

        public int? default_currency_id { get; set; }

        [StringLength(50)]
        public string? landed_cost_rule { get; set; }

        [StringLength(50)]
        public string? incoterm { get; set; }

        [StringLength(50)]
        public string? default_freight_method { get; set; }

        [StringLength(50)]
        public string? po_sending_method { get; set; }
        public int? default_country_of_loading_id { get; set; }
        public int? default_port_of_loading_id { get; set; }
        public int? default_country_of_discharge_id { get; set; }
        public int? default_port_of_discharge_id { get; set; }
    }
}
