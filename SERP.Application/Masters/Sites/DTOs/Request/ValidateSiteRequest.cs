using SERP.Domain.Common.Enums;

namespace SERP.Application.Masters.Sites.DTOs.Request
{
    public class ValidateSiteRequest
    {
        public string site_no { get; set; }
        public int? country_id { get; set; }
        public SERPEnum.Mode mode { get; set; }

    }
}
