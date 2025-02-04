using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;
using SERP.Domain.Transactions.Containers.Model;
using SERP.Domain.Transactions.InboundShipments.Model;
using SERP.Domain.Transactions.Invoice.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.AdvancedShipmentNotices
{
    public class ASNHeaderRepository : GenericRepository<ASNHeader>, IASNHeaderRepository
    {
        public ASNHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckAsnHeaderExisted(int id)
        {
            return await _dbContext.AsnHeaders.AnyAsync(x => x.id == id);
        }

        public async Task<List<ASNHeader>> GetAsnForMappingWithInboundShipment(HashSet<int> requestAsnList)
        {
            var query = _dbContext.AsnHeaders.Where(x => requestAsnList.Contains(x.id)
                                                         && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft
                                                         && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Closed
                                                         && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Cancelled);

            return await query.ToListAsync();
        }

        public async Task<List<ASNHeader>> GetNewAsnForMappingWithInboundShipment(HashSet<int> requestAsnList)
        {
            var query = _dbContext.AsnHeaders.Where(x => requestAsnList.Contains(x.id)
                                                 && x.status_flag == DomainConstant.AdvancedShipmentNotices.StatusFlag.New);

            return await query.ToListAsync();
        }

        public async Task<List<AsnHeaderDetail>> GetAsnInfoByInboundShipmentId(int inboundShipmentId)
        {
            var ishAsn = await (from inboundShipmentAsn in _dbContext.InboundShipmentASN.AsNoTracking()
                where inboundShipmentAsn.inbound_shipment_id == inboundShipmentId
                select inboundShipmentAsn.asn_header_id).ToListAsync();


            var invoiceQuery = from invoiceHeader in _dbContext.InvoiceHeaders.AsNoTracking()
                               join currency in _dbContext.Currency.AsNoTracking() on invoiceHeader.currency_id equals currency.id
                               where invoiceHeader.asn_header_id != null && ishAsn.Contains(invoiceHeader.asn_header_id.Value)
                               select new InvoiceAsnResponseDetail
                               {
                                   asn_header_id = invoiceHeader.asn_header_id,
                                   invoice_no = invoiceHeader.invoice_no,
                                   invoice_currency = currency.currency_code,
                                   invoice_amt = invoiceHeader.amt
                               };

            var containerQuery = from container in _dbContext.Containers.AsNoTracking()
                                 join containerAsn in _dbContext.ContainerASNs.AsNoTracking() on container.id equals containerAsn.container_id
                                 where ishAsn.Contains(containerAsn.asn_header_id)
                                 select new ContainerDetail
                                 {
                                     asn_header_id = containerAsn.asn_header_id,
                                     container_id = container.id,
                                     status_flag = container.status_flag,
                                      container_no = container.container_no,
                                     shipment_type = container.shipment_type,
                                     weight = container.weight,
                                     detention_date = container.detention_date
                                 };

            var result = await (from asnHeader in _dbContext.AsnHeaders.AsNoTracking()
                                join supplier in _dbContext.Supplier.AsNoTracking() on asnHeader.supplier_id equals supplier.id
                                where ishAsn.Contains(asnHeader.id)
                                select new AsnHeaderDetail
                                {
                                    id = asnHeader.id,
                                    asn_no = asnHeader.asn_no,
                                    supplier_no = supplier.supplier_no,
                                    supplier_name = supplier.supplier_name,
                                    invoice = invoiceQuery.Where(x => x.asn_header_id == asnHeader.id).ToList(),
                                    containers = containerQuery.Where(x => x.asn_header_id == asnHeader.id).ToList(),
                                }).ToListAsync();

            return result;
        }

        public async Task<int[]> CheckAsnHeaderForDelete(HashSet<int> asnHeaderIds)
        {
            var asnHeaderValid = await _dbContext.AsnHeaders.AsNoTracking()
                .Where(x => asnHeaderIds.Contains(x.id)
                            && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Draft
                            && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Closed
                            && x.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.Cancelled)
                .Select(x => new
                {
                    asn_header_id = x.id,
                }).ToListAsync();

            return asnHeaderValid.Select(x => x.asn_header_id).ToArray();
        }

        public async Task<List<AsnInvoiceResponseDetail>> GetAsnInvoiceAsync(int asnHeaderId)
        {
            var result = from invoiceHeader in _dbContext.InvoiceHeaders.AsNoTracking()
                         join invoiceCurrency in _dbContext.Currency.AsNoTracking() on invoiceHeader.currency_id equals invoiceCurrency.id

                         where invoiceHeader.asn_header_id == asnHeaderId
                         select new AsnInvoiceResponseDetail()
                         {
                             #region removed
                             //invoice_header = new AsnInvoiceHeaderModel
                             //{
                             //    id = invoiceHeader.id,
                             //    status_flag = invoiceHeader.status_flag,
                             //    invoice_no = invoiceHeader.invoice_no,
                             //    amt = invoiceHeader.amt,
                             //    total_amt = invoiceHeader.total_amt,
                             //    currency_id = invoiceHeader.currency_id,
                             //    //exchange_rate = invoiceHeader.exchange_rate,
                             //    //total_packages = invoiceHeader.total_packages,
                             //    //total_gross_weight = invoiceHeader.total_gross_weight,
                             //    //volume = invoiceHeader.volume,
                             //    variance_reason = invoiceHeader.variance_reason,
                             //},
                             //invoice_details = (from invoiceDetail in _dbContext.InvoiceDetails.AsNoTracking()
                             //    //join item in _dbContext.Item on invoiceDetail.item_id equals item.id
                             //    where invoiceDetail.invoice_header_id == invoiceHeader.id
                             //    select new AsnInvoiceDetailResponseModel
                             //    {
                             //        id = invoiceDetail.id,
                             //        //status_flag = invoiceDetail.status_flag,
                             //        line_no = invoiceDetail.line_no,
                             //        //unit_cost = invoiceDetail.unit_cost,
                             //        qty = invoiceDetail.qty,
                             //        //uom = invoiceDetail.uom,
                             //        //item = new ItemResponseDetail()
                             //        //{
                             //        //    item_id = invoiceDetail.item_id,
                             //        //    item_no = item.item_no,
                             //        //    description_1 = item.description_1,
                             //        //    primary_uom = item.primary_uom
                             //        //},
                             //        country_of_origin_id = invoiceDetail.country_of_origin_id,
                             //        //notes_to_warehouse = invoiceDetail.notes_to_warehouse,
                             //        //extended_cost = invoiceDetail.extended_cost,
                             //        po = new AsnPoResponseDetail()
                             //        {
                             //            po_detail_id = invoiceDetail.po_detail_id,
                             //        }
                             //    }).ToList() 
                             #endregion
                             id = invoiceHeader.id,
                             invoice_currency = invoiceCurrency.currency_code,
                             invoice_no = invoiceHeader.invoice_no,
                             total_invoice_amt = invoiceHeader.total_amt,
                             invoice_exchange_rate = 1 // TODO: need to confirm
                         };

            return await result.ToListAsync();
        }

        public async Task<List<ASNDetail>> GetAsnOfInvoiceDetail(List<int> invoiceDetailId)
        {
            var asnHeaderIds = from invoiceDetail in _dbContext.InvoiceDetails.AsNoTracking()
                               join invoiceHeader in _dbContext.InvoiceHeaders.AsNoTracking() on invoiceDetail.invoice_header_id equals invoiceHeader.id
                               where invoiceDetailId.Contains(invoiceDetail.id)
                               select invoiceHeader.asn_header_id;

            return await _dbContext.AsnDetails.AsNoTracking().Where(x => asnHeaderIds.Contains(x.asn_header_id)).ToListAsync();
        }
    }
}
