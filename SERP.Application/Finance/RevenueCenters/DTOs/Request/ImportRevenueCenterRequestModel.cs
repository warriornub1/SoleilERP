using Microsoft.AspNetCore.Http;

namespace SERP.Application.Finance.RevenueCenters.DTOs.Request
{
    public class ImportRevenueCenterRequestModel
    {
        public required IFormFile File { get; set; }
    }
}
