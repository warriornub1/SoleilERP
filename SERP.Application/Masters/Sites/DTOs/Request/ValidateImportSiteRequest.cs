using SERP.Application.Masters.Sites.DTOs.Response;
using SERP.Domain.Masters.Countries;

namespace SERP.Application.Masters.Sites.DTOs.Request
{
    public class ValidateImportSiteRequest
    {
        public List<ImportSiteData> ExcelData { get; set; }
        public List<Country> Countries { get; set; }
    }
}
