using System.ComponentModel.DataAnnotations;

namespace SERP.Application.Finance.CompanyStructures.DTOs.Response
{
    public class SearchCompanyStructureResponseModel
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public int sequence { get; set; }
        public string org_no { get; set; }
        public string org_code { get; set; }
        public string org_description { get; set; }
        public string status_flag { get; set; }
        public int org_type { get; set; }
        public bool top_flag { get; set; }
        public InChargeEmployee in_charge_employee { get; set; }
        public int? parent_id { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class InChargeEmployee
    {
        public int id { get; set; }
        public string employee_no { get; set; }
        public string employee_name { get; set; }
        public string alias { get; set; }
    }
}
