using SERP.Domain.Common;

namespace SERP.Domain.Finance.RevenueCenterCompanyMapping
{
    public class RevenueCenterCompanyMapping : BaseModel
    {
        public int id { get; set; }
        public int revenue_center_id { get; set; }
        public int company_id { get; set; }
    }
}
