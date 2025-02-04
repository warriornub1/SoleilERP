using SERP.Application.Masters.Countries.DTOs.Response;

namespace SERP.Application.Masters.Countries.DTOs.Request
{
    public class ValidateCountryRequest
    {
        public List<ImportCountryData> ExcelData { get; set; }
    }
}
