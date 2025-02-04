using Microsoft.AspNetCore.Http;

namespace SERP.Application.Finance.Natural_Accounts.DTOs.Request
{
    public class ImportNaturalAccountRequestModel
    {
        public required IFormFile File { get; set; }
    }
}
