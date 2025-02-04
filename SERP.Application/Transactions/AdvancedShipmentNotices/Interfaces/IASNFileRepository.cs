using SERP.Application.Common;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces
{
    public interface IASNFileRepository: IGenericRepository<ASNFile>
    {
        Task<List<AsnFileResponseDetail>> GetFileInfoAsync(int asnHeaderId);
        Task<Dictionary<int, bool>> HasAttachmentCheck(HashSet<int> asnHeaderIds);
    }
}
