namespace SERP.Application.Finance.Employees.DTOs.Response
{
    public class SearchEmployeeResponseModel
    {
        public int id { get; set; }
        public string employee_no { get; set; }
        public string employee_name { get; set; }
        public string ailias { get; set; }
        public string org_description { get; set; }
        public string location { get; set; }
        public string status_flag { get; set; }
        public string email { get; set; }
        public CompanyStructures company_structure { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }

    public class CompanyStructures
    {
        public List<Department> departments { get; set; }
        public List<Division> divisions { get; set; }
        public List<Section> sections { get; set; }
        public List<Team> teams { get; set; }
    }

    public class Department
    {
        public int department { get; set; }
    }
    public class Division
    {
        public int divison { get; set; }
    }
    public class Section
    {
        public int section { get; set; }
    }

    public class Team
    {
        public string team { get; set; }
    }

}
