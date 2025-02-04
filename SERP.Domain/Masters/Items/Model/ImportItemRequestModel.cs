using Microsoft.AspNetCore.Http;

namespace SERP.Domain.Masters.Items.Model
{
    public class ImportItemRequestModel
    {
        public IFormFile File { get; set; }
    }
}
