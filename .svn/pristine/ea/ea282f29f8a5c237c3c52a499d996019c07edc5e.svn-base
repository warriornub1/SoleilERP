using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Common
{
    public class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime created_on { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string created_by { get; set; }

        public DateTime? last_modified_on { get; set; }

        [StringLength(50)]
        public string? last_modified_by { get; set; }

    }
}
