using SERP.Domain.Masters.Countries.Models;

namespace SERP.Domain.Masters.Suppliers.Models
{
    public class PagedSupplierDetail
    {
        public int id { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public int registered_site_id { get; set; }
        public string registered_site_no { get; set; }
        public string registered_site_name { get; set; }
        public string status_flag { get; set; }
        public bool service_flag { get; set; }
        public bool product_flag { get; set; }
        public bool authorised_flag { get; set; }
        public string? payment_term { get; set; }
        public string? payment_method { get; set; }
        public int? default_currency_id { get; set; }
        public string? default_currency { get; set; }
        public string? landed_cost_rule { get; set; }
        public string? incoterm { get; set; }
        public string? default_freight_method { get; set; }
        public string? po_sending_method { get; set; }
        public CountryDetail? default_country_of_loading { get; set; }
        public PortInfoDetail? default_port_of_loading { get; set; }
        public CountryDetail? default_country_of_discharge { get; set; }
        public PortInfoDetail? default_port_of_discharge { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
