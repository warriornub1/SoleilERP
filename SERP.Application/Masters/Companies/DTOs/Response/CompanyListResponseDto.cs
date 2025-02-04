using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Application.Masters.Companies.DTOs.Response
{
    public class CompanyListResponseDto
    {
        public List<GetCompanyList> items { get; set; }
    }

    public class GetCompanyList
    {
        public int id { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set;}
    }
}
