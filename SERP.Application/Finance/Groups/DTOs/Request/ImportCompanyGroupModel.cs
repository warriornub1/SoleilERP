using Microsoft.AspNetCore.Http;

namespace SERP.Application.Finance.Groups.DTOs.Request
{
    public class ImportCompanyGroupModel
    {
        public required IFormFile File { get; set; }
    }
}
