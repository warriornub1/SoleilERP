using SERP.Application.Common;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;

namespace SERP.Application.Transactions.InboundShipments.Interfaces
{
    public interface IInboundShipmentFileRepository: IGenericRepository<InboundShipmentFile>
    {
        Task<List<InboundShipmentFileDetail>?> GetInboundShipmentFileDetail(int inboundShipmentId);
        Task<List<InboundShipmentFile>> GetInboundShipmentFiles(int inboundShipmentId);
    }
}
