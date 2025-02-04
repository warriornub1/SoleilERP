using SERP.Application.Common;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;

namespace SERP.Application.Transactions.InboundShipments.Interfaces
{
    public interface IInboundShipmentRepository: IGenericRepository<InboundShipment>
    {
        Task<bool> CheckExisted(int requestInboundShipmentId);
        Task<bool> IsValidInboundShipment(int inboundShipmentId);
        Task<int[]> CheckInvalidInboundShipment(HashSet<int> inboundShipmentIds);
        IQueryable<PagedIshResponseDetail> BuildISHFilterQuery(PagedFilterIsRequestModel request, out int i);
        Task<List<InboundShipment>> GetListInboundShipmentByAsnHeaderAsync(int asnHeaderId);
        Task<AsnInboundShipmentResponseDetail?> GetInboundShipmentByAsnHeaderAsync(int asnHeaderId, string bl_awb_provided);
    }
}
