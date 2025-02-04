using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Transactions.CustomViews
{
    public class CustomViewFilter: BaseModel
    {
        public int? custom_view_id { get; set; }
        [StringLength(50)]
        public string filter { get; set; }
        /// <summary>
        /// If there is more than 1 filter, delimit by pipe |
        /// For example Filter is Status and Filter Value is 00|01 Filter by Status with status of 00 and 01
        /// </summary>
        [StringLength(1024)]
        public string filter_value { get; set; }
    }
}
