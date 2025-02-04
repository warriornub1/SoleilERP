using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.Receiving.Interfaces;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;

namespace SERP.Infrastructure.Transactions.Receiving
{
    internal class ReceivingDetailRepository : GenericRepository<ReceivingDetail>, IReceivingDetailRepository
    {
        public ReceivingDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<ReceivingDetail>> GetReceivingDetailListAsync(List<int> receivingDetailIds, int rcvHeaderId)
        {
            return _dbContext.ReceivingDetails.Where(x => x.receiving_header_id == rcvHeaderId && receivingDetailIds.Contains(x.id)).ToList();
        }
        public async Task<List<ReceivingDetail>> GetDetailByReceivingHeaderIdAsync(int rcvHeaderId)
        {
            return await _dbContext.ReceivingDetails.Where(x => x.receiving_header_id == rcvHeaderId).OrderBy(x => x.line_no).ToListAsync();
        }
        public async Task<int> GetReceivingDetailLastLineNoAsync(int rcvHeaderId)
        {
            return _dbContext.ReceivingDetails.Where(x => x.receiving_header_id == rcvHeaderId).Select(x => (int?)x.line_no).Max() ?? 0;
        }
        public async Task<List<string>> GetDocumentListAsync(int rcvHeaderId)
        {
            return _dbContext.ReceivingDetails
                .Where(x => x.receiving_header_id == rcvHeaderId)
                .Where(x => x.supplier_document_no != null)
                .Select(x => x.supplier_document_no)
                .Distinct().OrderBy(x => x).ToList();
        }
        public async Task<List<ReceivingItemModel>> GetItemListAsync(List<Expression<Func<ReceivingItemModel, bool>>> cond, int rcvHeaderId, string docNo, string packageNo)
        {
            var query = (from rcvDetail in _dbContext.ReceivingDetails
                         join item in _dbContext.Item on rcvDetail.item_id equals item.id
                         join rcvHeader in _dbContext.ReceivingHeaders on rcvDetail.receiving_header_id equals rcvHeader.id
                         join supplierItemMapping in _dbContext.SupplierItemMapping
                            on new { rcvDetail.item_id, supplier_id= rcvHeader.supplier_id ?? 0 }
                            equals new { supplierItemMapping.item_id, supplierItemMapping.supplier_id } into supplierItemGroup
                         from supplierItemMapping in supplierItemGroup.DefaultIfEmpty() // Left join
                         join itemUomConversion in _dbContext.ItemUomConversion on rcvDetail.item_id equals itemUomConversion.id into conversionGroup
                         from itemUomConversion in conversionGroup.DefaultIfEmpty() // Left join
                         let packageQty = (
                            from detail in _dbContext.ReceivingDetails
                            where detail.receiving_header_id == rcvHeaderId && detail.package_no == packageNo && detail.supplier_document_no == docNo
                            group detail by new { detail.item_id, detail.package_no, detail.supplier_document_no } into g
                            select g.Sum(d => d.qty - d.inspected_qty)).FirstOrDefault()
                         let documentQty = (
                             from detail in _dbContext.ReceivingDetails
                             where detail.receiving_header_id == rcvHeaderId && detail.supplier_document_no == docNo
                             group detail by new { detail.item_id, detail.supplier_document_no } into g
                             select g.Sum(d => d.qty - d.inspected_qty)).FirstOrDefault()
                         let receivingQty = (
                             from detail in _dbContext.ReceivingDetails
                             where detail.receiving_header_id == rcvHeaderId
                             group detail by detail.item_id into g
                             select g.Sum(d => d.qty - d.inspected_qty)).FirstOrDefault()
                         select new ReceivingItemModel
                         {
                             receiving_header_id = rcvDetail.receiving_header_id,
                             package_no = rcvDetail.package_no,
                             document_no = rcvDetail.supplier_document_no,
                             item_id = rcvDetail.item_id,
                             item_no = item.item_no,
                             description_1 = item.description_1,
                             supplier_part_no = supplierItemMapping.supplier_part_no,
                             inspection_instruction = item.inspection_instruction,
                             item_uom_conversion = itemUomConversion == null
                                        ? null
                                        : new Item_Uom_Conversion
                                        {
                                            primary_uom_qty = itemUomConversion.primary_uom_qty,
                                            secondary_uom_qty = itemUomConversion.secondary_uom_qty
                                        },
                             primary_uom = item.primary_uom,
                             secondary_uom = item.secondary_uom,
                             package_qty = packageQty,
                             document_qty = documentQty,
                             receiving_qty = receivingQty
                         });

            if (cond.Any())
            {
                foreach (var condition in cond)
                {
                    query = query.Where(condition);
                }
            }
            query = query.OrderBy(x => x.item_no);

            var result = await query.Distinct().ToListAsync();
            return result;
        }

        public async Task<List<ReceivingDetail>> GetReceivingDetailLineByItemAsync(List<Expression<Func<ReceivingDetail, bool>>> cond)
        {
            var query = (from rcvDetail in _dbContext.ReceivingDetails
                         select new ReceivingDetail
                         {
                             id = rcvDetail.id,
                             line_no = rcvDetail.line_no,
                             receiving_header_id = rcvDetail.receiving_header_id,
                             item_id = rcvDetail.item_id
                         });

            if (cond.Any())
            {
                foreach (var condition in cond)
                {
                    query = query.Where(condition);
                }
            }
            query = query.OrderBy(x => x.line_no);

            var result = await query.ToListAsync();
            return result;
        }
        public async Task<ReceivingItemDetailUomConversionModel> GetItemDetailsAsync(int rcvDetailId)
        {
            var query = (from rcvDetail in _dbContext.ReceivingDetails
                         join item in _dbContext.Item on rcvDetail.item_id equals item.id
                         join poDetail in _dbContext.PoDetails on rcvDetail.po_detail_id equals poDetail.id
                         join itemUomConversion in _dbContext.ItemUomConversion on rcvDetail.item_id equals itemUomConversion.item_id into conversionGroup
                         from itemUomConversion in conversionGroup.DefaultIfEmpty()
                         join packingDetail in _dbContext.PackingDetails on 
                        new { packingHeaderID = rcvDetail.packing_header_id ?? 0, asnDetailId = rcvDetail.asn_detail_id ?? 0} equals 
                        new { packingHeaderID = packingDetail.packing_header_id, asnDetailId = packingDetail.asn_detail_id }  into packingDetailGroup
                         from packingDetail in packingDetailGroup.DefaultIfEmpty()
                         select new ReceivingItemDetailUomConversionModel
                         {
                             receiving_detail_id = rcvDetail.id,
                             uom = rcvDetail.uom,
                             po_uom = rcvDetail.po_uom,
                             remaining_qty = rcvDetail.qty - rcvDetail.inspected_qty,
                             qty = rcvDetail.qty,
                             primary_uom_qty = itemUomConversion == null ? 0 : itemUomConversion.primary_uom_qty,
                             secondary_uom_qty = itemUomConversion == null ? 0 : itemUomConversion.secondary_uom_qty,
                             notes_to_warehouse = poDetail.notes_to_warehouse,
                             country_of_origin_id = rcvDetail.country_of_origin_id,
                             lot_control_flag = item.lot_control_flag,
                             packing_list_info = packingDetail == null ? null : 
                             new packingListInfo
                             {
                                 no_of_carton = packingDetail.no_of_carton,
                                 unit_per_carton = packingDetail.unit_per_carton,
                                 mixed_carton_no = packingDetail.mixed_carton_no
                             } 
                         });
            query = query.Where(x => x.receiving_detail_id == rcvDetailId);

            var result = await query.FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<ReceivingItemDetailModel>> GetReceivingDetailsAsync(List<Expression<Func<ReceivingItemDetailModel, bool>>> cond)
        {
            var query = (from rcvDetail in _dbContext.ReceivingDetails
                         join item in _dbContext.Item on rcvDetail.item_id equals item.id
                         join rcvHeader in _dbContext.ReceivingHeaders on rcvDetail.receiving_header_id equals rcvHeader.id
                         join poDetail in _dbContext.PoDetails on rcvDetail.po_detail_id equals poDetail.id
                         join poHeader in _dbContext.PoHeaders on poDetail.po_header_id equals poHeader.id
                         join packingHeader in _dbContext.PackingHeaders on rcvDetail.packing_header_id equals packingHeader.id into packingHeaderGroup
                         from packingHeader in packingHeaderGroup.DefaultIfEmpty()
                         join supplierItemMapping in _dbContext.SupplierItemMapping
                            on new { rcvDetail.item_id, supplier_id = rcvHeader.supplier_id ?? 0 }
                            equals new { supplierItemMapping.item_id, supplierItemMapping.supplier_id } into supplierItemGroup
                         from supplierItemMapping in supplierItemGroup.DefaultIfEmpty() // Left join
                         select new ReceivingItemDetailModel
                         {
                             receiving_header_id = rcvHeader.id,
                             receiving_detail_id = rcvDetail.id,
                             line_no = rcvDetail.line_no,
                             po_no = poHeader.po_no,
                             po_line_no = poDetail.line_no,
                             notes_to_warehouse = poDetail.notes_to_warehouse,
                             po_open_qty = poDetail.open_qty,
                             po_uom = rcvDetail.po_uom,
                             status_flag = rcvDetail.status_flag,
                             item_no = item.item_no,
                             description_1 = item.description_1,
                             lot_label_required = item.label_required_flag,
                             supplier_part_no = supplierItemMapping.supplier_part_no,
                             packing_list_no = packingHeader.id.ToString(),
                             package_no = rcvDetail.package_no,
                             qty = rcvDetail.qty,
                             inspection_qty = rcvDetail.inspected_qty,
                             receiving_uom = rcvDetail.uom,
                             country_of_origin_id = rcvDetail.country_of_origin_id,
                             supplier_document_type = rcvDetail.supplier_document_type,
                             supplier_document_no = rcvDetail.supplier_document_no,
                             created_by = rcvDetail.created_by,
                             created_on = rcvDetail.created_on,
                             last_modified_by = rcvDetail.last_modified_by,
                             last_modified_on = rcvDetail.last_modified_on
                         }) ;
            if (cond.Any())
            {
                foreach (var filter in cond)
                    query = query.Where(filter);
            }

            var result = await query.ToListAsync();
            return result;
        }
    }
}
