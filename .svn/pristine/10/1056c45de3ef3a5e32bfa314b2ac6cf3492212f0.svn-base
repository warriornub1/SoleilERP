using SERP.Domain.Common;
using SERP.Domain.Common.Attributes;
using SERP.Domain.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Finance.Employees
{
    public class Employee : BaseModel
    {
        public int company_id { get; set; }

        [StringLength(10)]
        public string employee_no { get; set; }

        [StringLength(100)]
        public string employee_name { get; set; }

        [StringLength(100)]
        public string ailas { get; set; }

        [StringLength(100)]
        public string occupation_description { get; set; }

        [StringLength(50)]
        public string location { get; set; }

        [StringLength(1)]
        [AcceptValue(
          DomainConstant.StatusFlag.Enabled,
          DomainConstant.StatusFlag.Disabled
        )]
        public string status_flag { get; set; }

        [StringLength(100)]
        public string email { get; set; }
    }
}
