using SERP.Application.Common;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;

namespace SERP.Application.Transactions.Receiving.Interfaces
{
    public interface IReceivingFileRepository : IGenericRepository<ReceivingFile>
    {
        Task<List<ReceivingFileModel>> GetFileInfoAsync(int rcvHeaderId);
    }
}
