using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.Invoices.Interfaces;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.Invoice.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.Invoices
{
    internal class InvoiceDetailRepository : GenericRepository<InvoiceDetail>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<InvoiceDetailPagedResponseDetail> BuildInvoiceDetailPagedQuery(FilterInvoiceDetailPagedRequestModel request, out int totalRows)
        {
            var invoiceDetailQuery = _dbContext.InvoiceDetails.AsNoTracking();

            if (request.create_date_from.HasValue)
            {
                invoiceDetailQuery = invoiceDetailQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                invoiceDetailQuery = invoiceDetailQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            var invoiceHeaderQuery = _dbContext.InvoiceHeaders.AsNoTracking();
            if (request.invoice_id is not null && request.invoice_id.Count > 0)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => request.invoice_id.Contains(x.id));
            }

            if (request.status is not null && request.status.Count > 0)
            {
                invoiceHeaderQuery = invoiceHeaderQuery.Where(x => request.status.Contains(x.status_flag));
            }

            var poHeaderQuery = _dbContext.PoHeaders.AsNoTracking();
            if (request.po_header_id is not null && request.po_header_id.Count > 0)
            {
                poHeaderQuery = poHeaderQuery.Where(x => request.po_header_id.Contains(x.id));
            }

            var poQuery = from poHeader in poHeaderQuery
                          join poDetail in _dbContext.PoDetails.AsNoTracking() on poHeader.id equals poDetail.po_header_id
                          join item in _dbContext.Item.AsNoTracking() on poDetail.item_id equals item.id
                          join itemMapping in _dbContext.SupplierItemMapping.AsNoTracking() on item.id equals itemMapping.item_id
                          select new
                          {
                              poHeader.po_no,
                              poHeader.po_currency_id,
                              podetail_id = poDetail.id,
                              poDetail.line_no,
                              poDetail.uom,
                              poDetail.unit_cost,
                              item.item_no,
                              item.description_1,
                              item.primary_uom,
                              itemMapping.supplier_part_no,
                          };

            var invoiceQuery = from invoiceDetail in invoiceDetailQuery
                               join invoiceHeader in invoiceHeaderQuery on invoiceDetail.invoice_header_id equals invoiceHeader.id
                               join supplier in _dbContext.Supplier on invoiceHeader.supplier_id equals supplier.id
                               join invoiceCurrency in _dbContext.Currency on invoiceHeader.currency_id equals invoiceCurrency.id
                               join po in poQuery on invoiceDetail.po_detail_id equals po.podetail_id
                               join poCurrency in _dbContext.Currency on po.po_currency_id equals poCurrency.id
                               select new
                               {
                                   po,
                                   detail_id = invoiceDetail.id,
                                   invoiceHeaderId = invoiceHeader.id,
                                   invoiceHeader.asn_header_id,
                                   invoiceDetail.po_detail_id,
                                   invoiceHeader.invoice_no,
                                   invoice_line_no = invoiceDetail.line_no,
                                   invoiceDetail.qty,
                                   invoiceHeader.status_flag,
                                   invoiceDetail.exchange_rate,
                                   invoiceCurrency = invoiceCurrency.currency_code,
                                   poCurrency = poCurrency.currency_code,
                                   invoiceDetail.unit_price,
                                   invoiceDetail.total_price,
                                   invoiceDetail.country_of_origin_id,
                                   invoiceDetail.created_on,
                                   invoiceDetail.created_by,
                                   invoiceDetail.last_modified_on,
                                   invoiceDetail.last_modified_by
                               };

            // - Keywords are Item No, Description 1, Supplier Part No
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                invoiceQuery = invoiceQuery.Where(x =>
                    x.po.item_no.Contains(request.Keyword) ||
                    x.po.description_1.Contains(request.Keyword) ||
                    x.po.supplier_part_no.Contains(request.Keyword));
            }

            totalRows = invoiceQuery.Count();

            var query = from invoice in invoiceQuery
                        join asnHeader in _dbContext.AsnHeaders on invoice.asn_header_id equals asnHeader.id into asnHeaderGroup
                        from asnHeader in asnHeaderGroup.DefaultIfEmpty()
                        join countryOfOrigin in _dbContext.Country on invoice.country_of_origin_id equals countryOfOrigin.id into countryOfOriginGroup
                        from countryOfOrigin in countryOfOriginGroup.DefaultIfEmpty()
                        select new InvoiceDetailPagedResponseDetail
                        {
                            detail_id = invoice.detail_id,
                            invoice_no = invoice.invoice_no,
                            invoice_line_no = invoice.invoice_line_no,
                            asn_no = asnHeader.asn_no,
                            po_no = invoice.po.po_no,
                            po_line_no = invoice.po.line_no,
                            status_flag = invoice.status_flag,
                            item_no = invoice.po.item_no,
                            description_1 = invoice.po.description_1,
                            supplier_part_no = invoice.po.supplier_part_no,
                            qty = invoice.qty,
                            uom = invoice.po.uom,
                            country_of_origin = countryOfOrigin != null ?
                                new CountryBasicDetail
                                {
                                    id = countryOfOrigin.id,
                                    country_name = countryOfOrigin.country_name,
                                    country_alpha_code_two = countryOfOrigin.country_alpha_code_two,
                                    country_alpha_code_three = countryOfOrigin.country_alpha_code_three
                                } : null,
                            exchange_rate = invoice.exchange_rate,
                            unit_price = invoice.unit_price,
                            total_price = invoice.total_price,
                            po_currency = invoice.poCurrency,
                            invoice_currency = invoice.invoiceCurrency,
                            primary_uom = invoice.po.primary_uom,
                            invoice_header_id = invoice.invoiceHeaderId,
                            po_unit_cost = invoice.po.unit_cost,
                            created_on = invoice.created_on,
                            created_by = invoice.created_by,
                            last_modified_on = invoice.last_modified_on,
                            last_modified_by = invoice.last_modified_by
                        };

            return query;
        }
    }
}
