using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Currencies
{
    public class Currency : BaseModel
    {
        [StringLength(5)]
        public string currency_code { get; set; }

        [StringLength(100)]
        public string currency_description { get; set; }

        /// <summary>
        /// E: Enabled
        /// D: Disabled
        /// </summary>
        [StringLength(1)]
        public string status_flag { get; set; } = "E";
    }
}
