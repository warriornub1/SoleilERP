using SERP.Domain.Common;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Masters.Items.Model;

namespace SERP.Domain.Masters.Suppliers.Models
{
    public class SupplierDetail : BaseModel
    {
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
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

        public List<IntermediarySupplierDetail>? intermediary_suppliers { get; set; }
        public List<SecondarySupplierDetail>? secondary_suppliers { get; set; }
        public List<SelfCollectSiteDetail>? self_collect_sites { get; set; }
        public RegisteredSiteDetail? registered_site { get; set; }
        public List<ItemMappingDetail> item_mapping { get; set; }
    }

    public class IntermediarySupplierDetail
    {
        public int intermediary_supplier_id { get; set; }
        public string intermediary_supplier_no { get; set; }
        public string intermediary_supplier_name { get; set; }
        public string intermediary_supplier_status_flag { get; set; }
        public bool intermediary_supplier_default { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
    }

    public class SecondarySupplierDetail
    {
        public int secondary_supplier_id { get; set; }
        public string secondary_supplier_no { get; set; }
        public string secondary_supplier_name { get; set; }
        public string secondary_supplier_status_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class SelfCollectSiteDetail
    {
        public int site_id { get; set; }
        public string site_no { get; set; }
        public string site_name { get; set; }
        public string? address_line_1 { get; set; }
        public string? address_line_2 { get; set; }
        public string? address_line_3 { get; set; }
        public string? address_line_4 { get; set; }
        public string? postal_code { get; set; }
        public string? state_province { get; set; }
        public string? county { get; set; }
        public string city { get; set; }
        public CountryDetail country { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
    }

    public class RegisteredSiteDetail : SelfCollectSiteDetail;

    public class PortInfoDetail
    {
        public int port_id { get; set; }
        public string port_no { get; set; }
        public string port_name { get; set; }
    }
}
