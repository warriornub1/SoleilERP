using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Domain.Transactions.FilesTracking
{
    public class FileTracking : BaseModel
    {
        public string file_type { get; set; }
        [StringLength(100)]
        public string file_name { get; set; }
        [StringLength(100)]
        public string upload_source { get; set; }
        [StringLength(255)]
        public string url_path { get; set; }
        [StringLength(255)]
        public string? url_path_thumbnail { get; set; }
        
        [Column(TypeName = "decimal(5, 2)")]
        public decimal file_size { get; set; }
        /// <summary>
        /// From LOV
        /// Document Type (Advice Note, Delivery Note, etc)
        /// </summary>
        [StringLength(50)]
        public string? document_type { get; set; }
    }
}
