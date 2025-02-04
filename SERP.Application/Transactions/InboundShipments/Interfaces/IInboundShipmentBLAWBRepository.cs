using SERP.Application.Common;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;

namespace SERP.Application.Transactions.InboundShipments.Interfaces
{
    public interface IInboundShipmentBLAWBRepository: IGenericRepository<InboundShipmentBLAWB>
    {
        Task<List<InboundShipmentBlAwbDetail>> GetBlAwbAndContainers(int inboundShipmentId);
        Task<int[]> CheckInvalidInboundShipmentBlAwb(HashSet<int> blAwbIds);
        Task<List<InboundShipmentBLAWB>> GetBlAwbByInboundShipmentId(int inboundShipmentId);
    }
}
