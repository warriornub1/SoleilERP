namespace SERP.Application.Masters.Companies.DTOs
{
    public class CompanyExcelDto
    {
        public string? company_No { get; set; }
        public string? company_Name { get; set; }
        public string? parentGroup { get; set; }
        public string? baseCurrency { get; set; }
        public string? registered_site_no { get; set; }
        public string? company_registration_no { get; set; }
        public string? gst_vat_registration_no { get; set;}
        public string? tax_registration_no { get; set; }
        public string? intercompany_flag { get; set; }
        public string? dormant_flag { get; set; }
        public string? status_flag { get; set; }
    }
}
