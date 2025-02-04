namespace SERP.Application.Transactions.PurchaseOrders.DTOs.Response
{
    public class PoResponse
    {
        //         "id" : "value", 
        public int id { get; set; }

        //  "po_no" : "value", 
        public string po_no { get; set; }

        //  "status_flag" : "value", 
        public string status_flag { get; set; }
        //  "po_type" : "value", 
        public string po_type { get; set; }
        //  "po_date" : "value", 
        public DateTime po_date { get; set; }
        //  "business_unit_no" : "value",
        public string? company_no { get; set; }
        //  "business_unit_name" : "value", 
        public string? company_name { get; set; }
        //  "branch_plant_no" : "value", 
        public string issuing_branch_plant_no { get; set; }
        //  "branch_plant_name" : "value", 
        public string issuing_branch_plant_name { get; set; }
        //  "supplier_no" : "value",
        public string supplier_no { get; set; }
        //  "supplier_name" : "value",
        public string supplier_name { get; set; }
        //  "intermediary_supplier_no" : "value",
        public string? intermediary_supplier_no { get; set; }
        //  "intermediary_supplier_name" : "value",
        public string? intermediary_supplier_name { get; set; }
        //  "secondary_supplier_no" : "value",
        public string? secondary_supplier_no { get; set; }
        //  "secondary_supplier_name" : "value",
        public string? secondary_supplier_name { get; set; }
        //  "ship_to_branch_plant_no" : "value",
        public string? ship_to_branch_plant_no { get; set; }
        //  "ship_to_branch_plant_name" : "value",
        public string? ship_to_branch_plant_name { get; set; }
        //  "ship_to_site_no" : "value",
        public string? ship_to_site_no { get; set; }
        //  "ship_to_site_name" : "value",
        public string? ship_to_site_name { get; set; }
        //  "forwarder_agent_no" : "value",
        public string? forwarder_agent_no { get; set; }
        //  "forwarder_agent_name" : "value",
        public string? forwarder_agent_name { get; set; }
        //  "sales_order_no" : "value", 
        public string? sales_order_no { get; set; }
        //  "payment_term" : "value", 
        public string? payment_term { get; set; }
        //  "base_currency" : "value", 
        public string? base_currency { get; set; }
        //  "po_currency" : "value", 
        public string? po_currency { get; set; }
        //  "exchange_rate" : "value", 
        public decimal? exchange_rate { get; set; }
        //  "cost_rule" : "value", 
        public decimal? total_amt_base { get; set; }
        public decimal? total_amt_foreign { get; set; }
        public string? cost_rule { get; set; }
        //  "urgency_code" : "value", 
        public string? urgency_code { get; set; }
        //  "order_discount" : "value", 
        public decimal? order_discount { get; set; }
        //  "taken_by" : "value", 
        public string? taken_by { get; set; }
        //  "internal_remarks" : "value", 
        public string? internal_remarks { get; set; }
        //  "frieght_method" : "value", 
        public string? freight_method { get; set; }
        //  "self_collect_site_no" : "value",
        public string self_collect_site_no { get; set; }
        //  "self_collect_site_name" : "value",
        public string self_collect_site_name { get; set; }
        //  "port_of_discharge_no" : "value",
        public string port_of_discharge_no { get; set; }
        //  "port_of_discharge_name" : "value",
        public string port_of_discharge_name { get; set; }
        //  "send_method" : "value", 
        public string? send_method { get; set; }
        //  "quotation_reference" : "value", 
        public string? quotation_reference { get; set; }
        //  "supplier_acknowledgement_no" : "value", 
        public string? supplier_acknowledgement_no { get; set; }
        //  "supplier_marking_reference" : "value", 
        public string? supplier_marking_reference { get; set; }
        //  "notes_to_supplier" : "value", 
        public string? notes_to_supplier { get; set; }
        //  "requested_date" : "value", 
        public DateOnly? requested_date { get; set; }
        //  "quoted_ex_fac_date_earliest" : "value", 
        public DateOnly? quoted_ex_fac_date_earliest { get; set; }
        //  "quoted_ex_fac_date_latest" : "value", 
        public DateOnly? quoted_ex_fac_date_latest { get; set; }
        //  "ack_ex_fac_date" : "value", 
        public DateOnly? ack_ex_fac_date { get; set; }
        //  "forecast_ex_wh_date" : "value", 
        public DateOnly? forecast_ex_wh_date { get; set; }
        //  "collection_date" : "value", 
        public DateTime? collection_date_time { get; set; }
        //  "created_on" : "value",
        public DateTime? created_on { get; set; }
        //  "created_by" : "value",
        public string? created_by { get; set; }
        //  "last_modified_on" : "value",
        public DateTime? last_modified_on { get; set; }
        //  "last_modified_by" : "value"   
        public string? last_modified_by { get; set; }
        //public int? ship_to_branch_plant_id { get; set; }
        //public int? ship_to_site_id { get; set; }
        //public int? self_collect_site_id { get; set; }
        public int? port_of_discharge_id { get; set; }
        public int intermediary_supplier_id { get; set; }
        public int? secondary_supplier_id { get; set; }
        public int? forwarder_id { get; set; }
    }
}
