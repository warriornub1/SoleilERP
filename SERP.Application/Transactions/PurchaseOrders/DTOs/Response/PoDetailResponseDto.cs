namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Response
{

    public class PoDetailResponseDto
    {
        public PoHeaderResponse? po_header { get; set; }
        public List<PoDetailResponse>? po_detail { get; set; }
        public List<PoFileResponse>? po_file { get; set; }
    }

    public class PoHeaderResponse
    {
        public int id { get; set; }
        public string po_no { get; set; }
        public string status_flag { get; set; }
        public string po_type { get; set; }
        public DateTime po_date { get; set; }
        public int? company_id { get; set; }
        public string? company_no { get; set; }
        public string? company_name { get; set; }
        public int? branch_plant_id { get; set; }
        public string branch_plant_no { get; set; }
        public string branch_plant_name { get; set; }
        public int? supplier_id { get; set; }
        public string supplier_no { get; set; }
        public string supplier_name { get; set; }
        public int? intermediary_supplier_id { get; set; }
        public string? intermediary_supplier_no { get; set; }
        public string? intermediary_supplier_name { get; set; }
        public int? secondary_supplier_id { get; set; }
        public string? secondary_supplier_no { get; set; }
        public string? secondary_supplier_name { get; set; }
        public int? ship_to_branch_plant_id { get; set; }
        public string? ship_to_branch_plant_no { get; set; }
        public string? ship_to_branch_plant_name { get; set; }
        public int? ship_to_site_id { get; set; }
        public string? ship_to_site_no { get; set; }
        public string? ship_to_site_name { get; set; }
        public int? forwarder_agent_id { get; set; }
        public string? forwarder_agent_no { get; set; }
        public string? forwarder_agent_name { get; set; }
        public int? sales_order_id { get; set; }
        public string? sales_order_no { get; set; }
        public string? payment_term { get; set; }
        public string? incoterm { get; set; }
        public decimal? total_amt_base { get; set; }
        public decimal? total_amt_foreign { get; set; }
        public int? base_currency_id { get; set; }
        public string? base_currency { get; set; }
        public int? po_currency_id { get; set; }
        public string? po_currency { get; set; }
        public string? exchange_rate { get; set; }
        public string? cost_rule { get; set; }
        public string? urgency_code { get; set; }
        public decimal? order_discount { get; set; }
        public string? taken_by { get; set; }
        public string? internal_remarks { get; set; }
        public string? freight_method { get; set; }
        public int? self_collect_site_id { get; set; }
        public string? self_collect_site_no { get; set; }
        public string? self_collect_site_name { get; set; }
        public int? port_of_discharge_id { get; set; }
        public string? port_of_discharge_no { get; set; }
        public string? port_of_discharge_name { get; set; }
        public int? port_of_discharge_country_id { get; set; }
        public string? send_method { get; set; }
        public string? quotation_reference { get; set; }
        public string? supplier_acknowledgement_no { get; set; }
        public string? supplier_marking_reference { get; set; }
        public string? notes_to_supplier { get; set; }
        public DateOnly? requested_date { get; set; }
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }
        public DateOnly? quoted_ex_fac_date_latest { get; set; }
        public DateOnly? ack_ex_fac_date { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        public DateTime? collection_date_time { get; set; }
        public DateTime? created_on { get; set; }
        public string? created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class PoDetailResponse
    {
        public int detail_id { get; set; }
        public int line_no { get; set; }
        public string line_type { get; set; }
        public string status_flag { get; set; }
        public int? item_id { get; set; }
        public string? item_no { get; set; }
        public string description_1 { get; set; }
        public string description_2 { get; set; }
        public string po_item_description { get; set; }
        public string primary_uom { get; set; }
        public string secondary_uom { get; set; }
        public int open_qty { get; set; }
        public int qty { get; set; }
        public string uom { get; set; }
        public int? supplier_item_mapping_id { get; set; }
        public string supplier_part_no { get; set; }
        public string supplier_material_code { get; set; }
        public string supplier_material_description { get; set; }
        public int? ship_to_branch_plant_id { get; set; }
        public string ship_to_branch_plant_no { get; set; }
        public string ship_to_branch_plant_name { get; set; }
        public decimal? extended_cost { get; set; }
        public decimal unit_cost { get; set; }
        public string? cost_rule { get; set; }
        public decimal? unit_discount { get; set; }
        public int? secondary_supplier_id { get; set; }
        public string secondary_supplier_no { get; set; }
        public string secondary_supplier_name { get; set; }
        public string? supplier_acknowledgement_no { get; set; }
        public string? instruction_to_supplier { get; set; }
        public string? internal_reference { get; set; }
        public string? notes_to_warehouse { get; set; }
        public DateOnly? requested_date { get; set; }
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }
        public DateOnly? quoted_ex_fac_date_latest { get; set; }
        public DateOnly? ack_ex_fac_date { get; set; }
        public DateOnly? forecast_ex_wh_date { get; set; }
        public DateTime? collection_date_time { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }

    }

    public class PoFileResponse
    {
        public int id { get; set; }
        public string file_name { get; set; }
        public string url_path { get; set; }
        public string file_type { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
    }
}
