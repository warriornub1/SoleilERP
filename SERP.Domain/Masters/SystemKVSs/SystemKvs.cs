using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.SystemKVSs
{
    public class SystemKvs : BaseModel
    {
        [StringLength(200)]
        public string keyword { get; set; }

        [StringLength(500)]
        public string? varchar_value { get; set; }
        public int? number_value { get; set; }
        public DateTime? date_value { get; set; }
    }
}
