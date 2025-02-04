using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Masters.ApplicationTokens
{
    public class ApplicationToken : BaseModel
    {
        public string application_code { get; set; }

        [StringLength(100)]
        public string application_name { get; set; }

        [StringLength(50)]
        public string token_type { get; set; }

        [StringLength(1024)]
        public string? token { get; set; }
        public DateTime? issued_date { get; set; }
        public DateTime? expiry_date { get; set; }
    }
}
