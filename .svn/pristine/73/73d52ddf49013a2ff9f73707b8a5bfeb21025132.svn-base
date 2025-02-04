using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.Invoices.Interfaces;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.Invoice.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;

namespace SERP.Infrastructure.Transactions.Invoices
{
    internal class InvoiceHeaderRepository : GenericRepository<InvoiceHeader>, IInvoiceHeaderRepository
    {
        public InvoiceHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<InvoicePagedResponseDetail> BuildInvoicePagedQuery(FilterInvoicePagedRequestModel request, out int totalRows)
        {
            var invoiceHeaderQuery = _dbContext.InvoiceHeaders.AsNoTracking();

            if (request.create_date_from.HasValue)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            if (request.status is not null && request.status.Count > 0)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => request.status.Contains(x.status_flag));
            }

            if (request.supplier_id is not null && request.supplier_id.Count > 0)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => request.supplier_id.Contains(x.supplier_id));
            }

            var asnHeaderQuery = _dbContext.AsnHeaders.AsNoTracking();
            if (request.branch_plant_id is not null && request.branch_plant_id.Count > 0)
            {
                asnHeaderQuery = asnHeaderQuery.Where(x => request.branch_plant_id.Contains(x.branch_plant_id));
            }

            var invoiceQuery = from invoiceHeader in invoiceHeaderQuery
                               join currency in _dbContext.Currency.AsNoTracking() on invoiceHeader.currency_id equals currency.id
                               join branchPlant in _dbContext.BranchPlant.AsNoTracking() on invoiceHeader.branch_plant_id equals branchPlant.id
                               join supplier in _dbContext.Supplier.AsNoTracking() on invoiceHeader.supplier_id equals supplier.id
                               select new
                               {
                                   invoiceHeader.id,
                                   invoiceHeader.invoice_no,
                                   supplier.supplier_no,
                                   supplier.supplier_name,
                                   invoiceHeader.asn_header_id,
                                   invoiceHeader.receiving_header_id,
                                   invoiceHeader.status_flag,
                                   currency.currency_code,
                                   branchPlant.branch_plant_no,
                                   branchPlant.branch_plant_name,
                                   invoiceHeader.amt,
                                   invoiceHeader.total_amt,
                                   invoiceHeader.variance_reason,
                                   invoiceHeader.invoice_date,
                                   invoiceHeader.created_on,
                                   invoiceHeader.created_by,
                                   invoiceHeader.last_modified_on,
                                   invoiceHeader.last_modified_by
                               };

            // keyword are Supplier No, Supplier Name and Invoice No
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                invoiceQuery = invoiceQuery.Where(x =>
                    x.supplier_no.Contains(request.Keyword) ||
                    x.supplier_name.Contains(request.Keyword) ||
                    x.invoice_no.Contains(request.Keyword));
            }

            totalRows = invoiceQuery.Count();

            var query = from invoice in invoiceQuery
                        join asnHeader in asnHeaderQuery on invoice.asn_header_id equals asnHeader.id into lstAsnHeader
                        from asnHeader in lstAsnHeader.DefaultIfEmpty()
                        join receivingHeader in _dbContext.ReceivingHeaders.AsNoTracking() on invoice.receiving_header_id equals receivingHeader.id into lstReceiving
                        from receivingHeader in lstReceiving.DefaultIfEmpty()
                        select new InvoicePagedResponseDetail
                        {
                            id = invoice.id,
                            asn_no = asnHeader.asn_no,
                            invoice_no = invoice.invoice_no,
                            receiving_no = receivingHeader.receiving_no,
                            status_flag = invoice.status_flag,
                            supplier_no = invoice.supplier_no,
                            supplier_name = invoice.supplier_name,
                            branch_plant_no = invoice.branch_plant_no,
                            branch_plant_name = invoice.branch_plant_name,
                            currency = invoice.currency_code,
                            amt = invoice.amt,
                            total_amt = invoice.total_amt,
                            variance_reason = invoice.variance_reason,
                            invoice_date = invoice.invoice_date,
                            created_on = invoice.created_on,
                            created_by = invoice.created_by,
                            last_modified_on = invoice.last_modified_on,
                            last_modified_by = invoice.last_modified_by
                        };

            return query;
        }

        public async Task<InvoiceInfoResponseModel?> GetInvoiceAsync(Expression<Func<InvoiceHeader, bool>> condition)
        {
            var invoiceHeaderQuery = _dbContext.InvoiceHeaders.AsNoTracking().Where(condition);

            var invoiceHeaderResponse = await (from invoiceHeader in invoiceHeaderQuery
                                                   //join currency in _dbContext.Currency.AsNoTracking() on invoiceHeader.currency_id equals currency.id
                                               join asnHeader in _dbContext.AsnHeaders.AsNoTracking() on invoiceHeader.asn_header_id equals asnHeader.id into lstAsnHeader
                                               from asnHeader in lstAsnHeader.DefaultIfEmpty()
                                               join receivingHeader in _dbContext.ReceivingHeaders.AsNoTracking() on invoiceHeader.receiving_header_id equals receivingHeader.id into lstReceiving
                                               from receivingHeader in lstReceiving.DefaultIfEmpty()
                                               select new InvoiceHeaderResponseModel
                                               {
                                                   id = invoiceHeader.id,
                                                   invoice_no = invoiceHeader.invoice_no,
                                                   asn_no = asnHeader.asn_no,
                                                   receiving_no = receivingHeader.receiving_no,
                                                   status_flag = invoiceHeader.status_flag,
                                                   supplier_id = invoiceHeader.supplier_id,
                                                   invoice_date = invoiceHeader.invoice_date,
                                                   branch_plant_id = invoiceHeader.branch_plant_id,
                                                   invoice_currency_id = invoiceHeader.currency_id,
                                                   amt = invoiceHeader.amt,
                                                   total_amt = invoiceHeader.total_amt,
                                                   variance_reason = invoiceHeader.variance_reason,
                                                   created_on = invoiceHeader.created_on,
                                                   created_by = invoiceHeader.created_by,
                                                   last_modified_on = invoiceHeader.last_modified_on,
                                                   last_modified_by = invoiceHeader.last_modified_by
                                               }).FirstOrDefaultAsync();

            if (invoiceHeaderResponse is null)
            {
                return null;
            }

            var invoiceDetailResponse = await (from invoiceDetail in _dbContext.InvoiceDetails.AsNoTracking().Where(x => x.invoice_header_id == invoiceHeaderResponse.id)
                                               join poDetail in _dbContext.PoDetails.AsNoTracking() on invoiceDetail.po_detail_id equals poDetail.id
                                                   into lstPoDetail
                                               from poDetail in lstPoDetail.DefaultIfEmpty()
                                               join poHeader in _dbContext.PoHeaders.AsNoTracking() on poDetail.po_header_id equals poHeader.id
                                               join item in _dbContext.Item.AsNoTracking() on poDetail.item_id equals item.id
                                               join itemMapping in _dbContext.SupplierItemMapping.AsNoTracking() on item.id equals itemMapping.item_id
                                               select new InvoiceDetailResponseModel
                                               {
                                                   id = invoiceDetail.id,
                                                   invoice_line_no = invoiceDetail.line_no,
                                                   po_detail_id = invoiceDetail.po_detail_id,
                                                   po_no = poHeader.po_no,
                                                   po_line_no = poDetail.line_no,
                                                   item_id = poDetail.item_id,
                                                   item_no = item.item_no,
                                                   description_1 = item.description_1,
                                                   supplier_part_no = itemMapping.supplier_part_no,
                                                   qty = invoiceDetail.qty,
                                                   uom = poDetail.uom,
                                                   exchange_rate = invoiceDetail.exchange_rate,
                                                   country_of_origin_id = invoiceDetail.country_of_origin_id,
                                                   unit_price = invoiceDetail.unit_price,
                                                   total_price = invoiceDetail.total_price,
                                                   po_currency_id = poHeader.po_currency_id,
                                                   po_unit_cost = poDetail.unit_cost,
                                                   po_open_qty = poDetail.open_qty,
                                                   created_on = invoiceDetail.created_on,
                                                   created_by = invoiceDetail.created_by,
                                                   last_modified_on = invoiceDetail.last_modified_on,
                                                   last_modified_by = invoiceDetail.last_modified_by
                                               }).ToListAsync();

            var invoiceFileResponse = await (from invoiceFile in _dbContext.InvoiceFiles.AsNoTracking().Where(x => x.invoice_header_id == invoiceHeaderResponse.id)
                                             join fileTracking in _dbContext.FilesTracking.AsNoTracking() on invoiceFile.file_id equals fileTracking.id
                                             select new InvoiceFileResponseModel
                                             {
                                                 id = invoiceFile.id,
                                                 file_name = fileTracking.file_name,
                                                 url_path = fileTracking.url_path,
                                                 document_type = fileTracking.document_type,
                                                 file_type = fileTracking.file_type,
                                                 created_by = invoiceFile.created_by,
                                                 created_on = invoiceFile.created_on
                                             }).ToListAsync();

            return new InvoiceInfoResponseModel
            {
                invoice_header = invoiceHeaderResponse,
                invoice_details = invoiceDetailResponse,
                invoice_files = invoiceFileResponse
            };
        }

        public async Task<bool> CheckInvoiceHeaderExistedAsync(int headerId)
        {
            return await _dbContext.InvoiceHeaders.AsNoTracking().AnyAsync(x => x.id == headerId);
        }
    }
}
