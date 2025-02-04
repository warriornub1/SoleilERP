using SERP.Application.Common;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;
using SERP.Domain.Transactions.Invoice.Model;

namespace SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces
{
    public interface IASNHeaderRepository: IGenericRepository<ASNHeader>
    {
        Task<bool> CheckAsnHeaderExisted(int id);
        Task<List<ASNHeader>> GetAsnForMappingWithInboundShipment(HashSet<int> requestAsnList);
        Task<List<ASNHeader>> GetNewAsnForMappingWithInboundShipment(HashSet<int> requestAsnList);
        Task<List<AsnHeaderDetail>> GetAsnInfoByInboundShipmentId(int inboundShipmentId);
        /// <summary>
        /// check asn header for delete
        /// </summary>
        /// <param name="asnHeaderIds"></param>
        /// <returns>
        /// <para>Item1: asn_header_id</para>
        /// <para>Item2: inbound_shipment_request_id</para>
        /// </returns>
        Task<int[]> CheckAsnHeaderForDelete(HashSet<int> asnHeaderIds);

        Task<List<AsnInvoiceResponseDetail>> GetAsnInvoiceAsync(int asnHeaderId);
        Task<List<ASNDetail>> GetAsnOfInvoiceDetail(List<int> invoiceDetailId);
    }
}
