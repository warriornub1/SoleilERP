using Microsoft.AspNetCore.Http;

namespace SERP.Domain.Masters.Suppliers.Models
{
    public class ImportSupplierRequestModel
    {
        public IFormFile File { get; set; }
    }
}
