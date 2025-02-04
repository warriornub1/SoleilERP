using SERP.Domain.Masters.Sites.Model.Base;
using System.ComponentModel.DataAnnotations;

namespace SERP.Domain.Masters.Sites.Model
{
    public class CreateSiteRequestModel : SiteRequestModel
    {
        [Required] 
        public new string site_no { get; set; }
    }
}
