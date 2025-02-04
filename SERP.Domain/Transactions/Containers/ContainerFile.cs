using SERP.Domain.Common;

namespace SERP.Domain.Transactions.Containers
{
    public class ContainerFile : BaseModel
    {
        public int container_id { get; set; }
        public string container_file_type { get; set; }
        public int file_id { get; set; }
    }
}
