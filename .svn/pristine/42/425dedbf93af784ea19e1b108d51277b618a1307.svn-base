using SERP.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Sites
{
    public class SitePurpose : BaseModel
    {
        /// <summary>
        /// Site ID	FK from table Site
        /// </summary>
        public int site_id { get; set; }
        /// <summary>
        /// Purpose of Site (LOV Value)
        /// </summary>
        [StringLength(50)]
        public string purpose { get; set; }
    }
}
