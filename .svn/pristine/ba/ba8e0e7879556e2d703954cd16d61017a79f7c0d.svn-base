using Microsoft.AspNetCore.Http;

namespace SERP.Application.Transactions.Containers.DTOs.Request
{
    public class UploadContainerFileDto
    {
        public int container_id { get; set; }

        public List<ContainerFilesDto> files { get; set; }
    }

    public class ContainerFilesDto
    {
        public string container_file_type { get; set; }
        public IFormFile file { get; set; }
    }
}
