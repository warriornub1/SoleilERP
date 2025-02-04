using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Domain.Transactions.AdvancedShipmentNotices.Model;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Domain.Transactions.PurchaseOrders.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Infrastructure.Transactions.PurchaseOrders
{
    internal class PODetailRepository : GenericRepository<PODetail>, IPODetailRepository
    {
        public PODetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Dictionary<int, List<int>>> GetSupplierIdsAsync(HashSet<int> poDetailIds)
        {
            var query = (from poDetail in _dbContext.PoDetails
                         join poHeader in _dbContext.PoHeaders on poDetail.po_header_id equals poHeader.id
                         where poDetailIds.Contains(poDetail.id)
                         group poDetail by poHeader.supplier_id into gr
                         select new
                         {
                             supplierId = gr.Key,
                             poDetailIDs = gr.Select(x => x.id).ToList()
                         });

            Dictionary<int, List<int>> result = await query.ToDictionaryAsync(x => x.supplierId, x => x.poDetailIDs);
            return result;
        }

        public HashSet<int> GetPoHeaderContainItem(List<int> requestItemIDs)
        {
            var query = (from poDetail in _dbContext.PoDetails
                         where requestItemIDs.Contains(poDetail.item_id) && !poDetail.status_flag.Equals(PurchaseOrder.StatusFlag.Cancelled)
                         select poDetail.po_header_id).Distinct();

            return query.ToHashSet();
        }

        public async Task<int[]> GetPoHeaderWithOpenQtyAsync(HashSet<string>? poHeaderStatus)
        {
            var query = _dbContext.PoDetails.Where(x => x.open_qty > 0);

            if (poHeaderStatus != null)
            {
                query = query.Where(x => poHeaderStatus.Contains(x.status_flag));
            }

            return await query.Select(x => x.po_header_id).Distinct().ToArrayAsync();
        }

        public async Task<int[]> GetPoDetailAvailable(HashSet<int> poDetailIDs, List<string> validStatus)
        {
            return await _dbContext.PoDetails
                .Where(x => poDetailIDs.Contains(x.id) && validStatus.Contains(x.status_flag))
                .Select(x => x.id).ToArrayAsync();
        }
        public async Task<List<PODetail>> GetPoDetailList(List<int> poDetailIDs)
        {
            return await _dbContext.PoDetails
                .Where(x => poDetailIDs.Contains(x.id)
                            && x.status_flag != PurchaseOrder.StatusFlag.Closed ||
                            x.status_flag != PurchaseOrder.StatusFlag.Cancelled)
                .Select(x => new PODetail
                {
                    id = x.id,
                    item_id = x.item_id,
                    uom = x.uom,
                }).ToListAsync();
        }
        public async Task<Dictionary<int, AsnPoResponseDetail>> GetDictionaryPoDataAsync(HashSet<int> poDetailIds)
        {
            return await (from poDetail in _dbContext.PoDetails
                          join poHeader in _dbContext.PoHeaders on poDetail.po_header_id equals poHeader.id
                          join poCurrency in _dbContext.Currency on poHeader.po_currency_id equals poCurrency.id
                          where poDetailIds.Contains(poDetail.id)
                          select new AsnPoResponseDetail
                          {
                              po_detail_id = poDetail.id,
                              po_header_id = poDetail.po_header_id,
                              po_no = poHeader.po_no,
                              po_line_no = poDetail.line_no,
                              po_open_qty = poDetail.open_qty,
                              po_currency = poCurrency.currency_code,
                              po_unit_cost = poDetail.unit_cost
                          }).ToDictionaryAsync(x => x.po_detail_id);
        }

        /// <summary>
        /// Build Query for filter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public IQueryable<PoResponseDetail> BuildPoFilterQuery(PagedFilterPoRequestModel request, out int totalRows)
        {
            #region Filter

            var poHeaderQuery = _dbContext.PoHeaders.AsNoTracking();

            // item
            if (request.Items is not null && request.Items.Count > 0)
            {
                var poHeaderIdsContainItem = GetPoHeaderContainItem(request.Items);
                poHeaderQuery = poHeaderQuery.Where(x => poHeaderIdsContainItem.Contains(x.id));
            }

            // status
            if (request.Statuses is not null && request.Statuses.Count > 0)
            {
                poHeaderQuery = poHeaderQuery.Where(x => request.Statuses.Contains(x.status_flag));
            }

            // create date from
            if (request.create_date_from.HasValue)
            {
                poHeaderQuery = poHeaderQuery.Where(x => x.created_on >= request.create_date_from);
            }

            // create date to
            if (request.create_date_to.HasValue)
            {
                poHeaderQuery = poHeaderQuery.Where(x => x.created_on <= request.create_date_to);
            }

            var supplierQuery = _dbContext.Supplier.AsNoTracking();

            // supplier
            if (request.Suppliers is not null && request.Suppliers.Count > 0)
            {
                supplierQuery = supplierQuery.Where(x => request.Suppliers.Contains(x.id));
            }


            // branch plant
            var issuingBranchPlantQuery = _dbContext.BranchPlant.AsNoTracking();
            if (request.BranchPlants is not null && request.BranchPlants.Count > 0)
            {
                issuingBranchPlantQuery = issuingBranchPlantQuery.Where(x => request.BranchPlants.Contains(x.id));
            }

            #endregion

            var mandatoryDataQuery = from poHeader in poHeaderQuery
                                     join supplier in supplierQuery on poHeader.supplier_id equals supplier.id
                                     join branchPlant in issuingBranchPlantQuery on poHeader.branch_plant_id equals branchPlant.id
                                     join company in _dbContext.Company.AsNoTracking() on branchPlant.company_id equals company.id
                                     join poCurrency in _dbContext.Currency.AsNoTracking() on poHeader.po_currency_id equals poCurrency.id
                                     join baseCurrency in _dbContext.Currency.AsNoTracking() on poHeader.base_currency_id equals baseCurrency.id
                                     select new
                                     {
                                         poHeader,
                                         supplier,
                                         branchPlant,
                                         company,
                                         poCurrency,
                                         baseCurrency
                                     };

            // Keywords are PO No, Supplier No, Supplier Name, Customer No and Customer Name.
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x =>
                    x.poHeader.po_no.Contains(request.Keyword) ||
                    x.supplier.supplier_no.Contains(request.Keyword) ||
                    x.supplier.supplier_name.Contains(request.Keyword) ||
                    x.branchPlant.branch_plant_no.Contains(request.Keyword) ||
                    x.branchPlant.branch_plant_name.Contains(request.Keyword));
            }

            totalRows = mandatoryDataQuery.Count();

            var query = from mainQuery in mandatoryDataQuery

                        join shipToBranchPlant in _dbContext.BranchPlant.AsNoTracking() on mainQuery.poHeader.ship_to_branch_plant_id equals shipToBranchPlant.id into lstShipToBranchPlant
                        from shipToBranchPlant in lstShipToBranchPlant.DefaultIfEmpty()

                        join shipToSite in _dbContext.Site.AsNoTracking() on mainQuery.poHeader.ship_to_site_id equals shipToSite.id into lstShipToSite
                        from shipToSite in lstShipToSite.DefaultIfEmpty()

                        join selfCollectSite in _dbContext.Site.AsNoTracking() on mainQuery.poHeader.self_collect_site_id equals selfCollectSite.id into lstSelfCollectSite
                        from selfCollectSite in lstSelfCollectSite.DefaultIfEmpty()

                        join portOfDischarge in _dbContext.Ports.AsNoTracking() on mainQuery.poHeader.port_of_discharge_id equals portOfDischarge.id into lstPortOfDischarge
                        from portOfDischarge in lstPortOfDischarge.DefaultIfEmpty()

                        join intermediarySupplier in _dbContext.Supplier.AsNoTracking() on mainQuery.poHeader.intermediary_supplier_id equals intermediarySupplier.id into lstIntermediarySupplier
                        from intermediarySupplier in lstIntermediarySupplier.DefaultIfEmpty()

                        join secondarySupplier in _dbContext.SecondarySupplier.AsNoTracking() on mainQuery.poHeader.secondary_supplier_id equals secondarySupplier.id into lstSecondarySupplier
                        from secondarySupplier in lstSecondarySupplier.DefaultIfEmpty()

                        join forwarder in _dbContext.Agent.AsNoTracking() on mainQuery.poHeader.forwarder_agent_id equals forwarder.id into lstForwarder
                        from forwarder in lstForwarder.DefaultIfEmpty()
                        select new PoResponseDetail()
                        {
                            id = mainQuery.poHeader.id,
                            po_no = mainQuery.poHeader.po_no,
                            status_flag = mainQuery.poHeader.status_flag,
                            po_type = mainQuery.poHeader.po_type,
                            po_date = mainQuery.poHeader.po_date,
                            company_no = mainQuery.company.company_no,
                            company_name = mainQuery.company.company_name,
                            branch_plant_no = mainQuery.branchPlant.branch_plant_no,
                            branch_plant_name = mainQuery.branchPlant.branch_plant_name,
                            supplier_no = mainQuery.supplier.supplier_no,
                            supplier_name = mainQuery.supplier.supplier_name,
                            sales_order_no = null, // TODO: Add sales order no later
                            payment_term = mainQuery.poHeader.payment_term,
                            incoterm = mainQuery.poHeader.incoterm,
                            total_amt_base = mainQuery.poHeader.total_amt_base,
                            total_amt_foreign = mainQuery.poHeader.total_amt_foreign,
                            total_line_discount = mainQuery.poHeader.total_line_discount,
                            exchange_rate = mainQuery.poHeader.exchange_rate,
                            cost_rule = mainQuery.poHeader.cost_rule,
                            urgency_code = mainQuery.poHeader.urgency_code,
                            order_discount = mainQuery.poHeader.order_discount,
                            taken_by = mainQuery.poHeader.taken_by,
                            internal_remarks = mainQuery.poHeader.internal_remarks,
                            freight_method = mainQuery.poHeader.freight_method,
                            send_method = mainQuery.poHeader.send_method,
                            quotation_reference = mainQuery.poHeader.quotation_reference,
                            supplier_acknowledgement_no = mainQuery.poHeader.supplier_acknowledgement_no,
                            supplier_marking_reference = mainQuery.poHeader.supplier_marking_reference,
                            notes_to_supplier = mainQuery.poHeader.notes_to_supplier,
                            requested_date = mainQuery.poHeader.requested_date,
                            quoted_ex_fac_date_earliest = mainQuery.poHeader.quoted_ex_fac_date_earliest,
                            quoted_ex_fac_date_latest = mainQuery.poHeader.quoted_ex_fac_date_latest,
                            ack_ex_fac_date = mainQuery.poHeader.ack_ex_fac_date,
                            forecast_ex_wh_date = mainQuery.poHeader.forecast_ex_wh_date,
                            collection_date_time = mainQuery.poHeader.collection_date_time,
                            created_on = mainQuery.poHeader.created_on,
                            created_by = mainQuery.poHeader.created_by,
                            last_modified_on = mainQuery.poHeader.last_modified_on,
                            last_modified_by = mainQuery.poHeader.last_modified_by,

                            base_currency = mainQuery.baseCurrency.currency_code,
                            po_currency = mainQuery.poCurrency.currency_code,

                            attachment_flag = mainQuery.poHeader.attachment_flag,
                            notes_to_warehouse_flag = mainQuery.poHeader.notes_to_warehouse_flag,

                            //ship_to_branch_plant_id = poHeader.ship_to_branch_plant_id,
                            ship_to_branch_plant_no = shipToBranchPlant != null ? shipToBranchPlant.branch_plant_no : string.Empty,
                            ship_to_branch_plant_name = shipToBranchPlant != null ? shipToBranchPlant.branch_plant_name : string.Empty,
                            //ship_to_site_id = poHeader.ship_to_site_id,
                            ship_to_site_no = shipToSite != null ? shipToSite.site_no : string.Empty,
                            ship_to_site_name = shipToSite != null ? shipToSite.site_name : string.Empty,

                            //self_collect_site_id = poHeader.self_collect_site_id,
                            self_collect_site_no = selfCollectSite != null ? selfCollectSite.site_no : string.Empty,
                            self_collect_site_name = selfCollectSite != null ? selfCollectSite.site_name : string.Empty,

                            //port_of_discharge_id = poHeader.port_of_discharge_id,
                            port_of_discharge_no = portOfDischarge.port_no,
                            port_of_discharge_name = portOfDischarge.port_name,

                            //intermediary_supplier_id = poHeader.intermediary_supplier_id,
                            intermediary_supplier_no = intermediarySupplier != null ? intermediarySupplier.supplier_no : string.Empty,
                            intermediary_supplier_name = intermediarySupplier != null ? intermediarySupplier.supplier_name : string.Empty,

                            //secondary_supplier_id = poHeader.secondary_supplier_id,
                            secondary_supplier_no = secondarySupplier != null ? secondarySupplier.sec_supplier_no : string.Empty,
                            secondary_supplier_name = secondarySupplier != null ? secondarySupplier.sec_supplier_name : string.Empty,

                            //forwarder_id = poHeader.forwarder_id
                            forwarder_agent_no = forwarder != null ? forwarder.agent_no : string.Empty,
                            forwarder_agent_name = forwarder != null ? forwarder.agent_name : string.Empty
                        };

            // TODO: - Customer No and Customer Name is only valid if Sales Order No is identified.  (waiting add SalesOrder table)

            return query;
        }

        public List<PODetail> GetPoDetailByHeaderIdAsync(int poHeaderId)
        {
            return _dbContext.PoDetails.Where(x => x.po_header_id == poHeaderId).ToList();
        }

        public IQueryable<PagePoDetailResponseDetail> BuildPoDetailFilterQuery(PagedFilterPoRequestModel request, out int totalRows)
        {
            var mandatoryDataQuery = from poDetail in _dbContext.PoDetails.AsNoTracking()
                                     join item in _dbContext.Item.AsNoTracking() on poDetail.item_id equals item.id
                                     join poHeader in _dbContext.PoHeaders.AsNoTracking() on poDetail.po_header_id equals poHeader.id
                                     join supplierItemMapping in _dbContext.SupplierItemMapping.AsNoTracking() on poDetail.supplier_item_mapping_id equals supplierItemMapping.id into supplierItemMappingGroup
                                     from supplierItemMapping in supplierItemMappingGroup.DefaultIfEmpty()
                                     select new
                                     {
                                         poDetail,
                                         item,
                                         poHeader,
                                         lot_label_required_flag = item.label_required_flag,
                                         supplier_part_no = supplierItemMapping != null ? supplierItemMapping.supplier_part_no : null,
                                         supplier_material_code = supplierItemMapping != null ? supplierItemMapping.supplier_material_code : null,
                                         supplier_material_description = supplierItemMapping != null ? supplierItemMapping.supplier_material_description : null
                                     };

            // Apply filters
            if (request.PoHeaderIDs != null && request.PoHeaderIDs.Any())
            {
                var distinctPoHeaderIds = request.PoHeaderIDs.Distinct().ToList();
                mandatoryDataQuery = mandatoryDataQuery.Where(x => distinctPoHeaderIds.Contains(x.poDetail.po_header_id));
            }

            // SE-127 - search branch_plant_id from POHeader.ship_to_branch_plant_id
            if (request.BranchPlants is not null && request.BranchPlants.Count > 0)
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x =>
                    x.poHeader.ship_to_branch_plant_id != null && request.BranchPlants.Contains(x.poHeader.ship_to_branch_plant_id.Value));
            }

            if (!string.IsNullOrEmpty(request.line_type))
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x => x.poDetail.line_type == request.line_type);
            }

            if (request.Statuses is not null && request.Statuses.Count > 0)
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x => request.Statuses.Contains(x.poDetail.status_flag));
            }

            if (request.Suppliers is not null && request.Suppliers.Count > 0)
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x => request.Suppliers.Contains(x.poHeader.supplier_id));
            }

            if (!request.AllowZeroOpenQty)
            {
                // Do not show PO Open Qty that is 0
                mandatoryDataQuery = mandatoryDataQuery.Where(x => x.poDetail.open_qty > 0);
            }

            // - Keyword are Item No, Supplier Part No, PO No, Description 1 and PO Item Description
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                mandatoryDataQuery = mandatoryDataQuery.Where(x =>
                    EF.Functions.Like(x.item.item_no, $"%{request.Keyword}%") ||
                    EF.Functions.Like(x.poHeader.po_no, $"%{request.Keyword}%") ||
                    EF.Functions.Like(x.item.description_1, $"%{request.Keyword}%") ||
                    EF.Functions.Like(x.supplier_part_no, $"%{request.Keyword}%") ||
                    EF.Functions.Like(x.poDetail.po_item_description, $"%{request.Keyword}%"));
            }

            totalRows = mandatoryDataQuery.Count();

            return from mainQuery in mandatoryDataQuery
                   join poCurrency in _dbContext.Currency.AsNoTracking() on mainQuery.poHeader.po_currency_id equals poCurrency.id
                   join shipToBranchPlant in _dbContext.BranchPlant.AsNoTracking() on mainQuery.poDetail.ship_to_branch_plant_id equals shipToBranchPlant.id into lstShipToBranchPlant
                   from shipToBranchPlant in lstShipToBranchPlant.DefaultIfEmpty()
                   join secondarySupplier in _dbContext.SecondarySupplier.AsNoTracking() on mainQuery.poDetail.secondary_supplier_id equals secondarySupplier.id into lstSecondarySupplier
                   from secondarySupplier in lstSecondarySupplier.DefaultIfEmpty()
                   select new PagePoDetailResponseDetail()
                   {
                       po_header_id = mainQuery.poDetail.po_header_id,
                       detail_id = mainQuery.poDetail.id,
                       line_no = mainQuery.poDetail.line_no,
                       line_type = mainQuery.poDetail.line_type,
                       qty = mainQuery.poDetail.qty,
                       open_qty = mainQuery.poDetail.open_qty,
                       status_flag = mainQuery.poDetail.status_flag,
                       po_item_description = mainQuery.poDetail.po_item_description,
                       po_no = mainQuery.poHeader.po_no,
                       uom = mainQuery.poDetail.uom,
                       supplier_id = mainQuery.poHeader.supplier_id,
                       po_currency_id = mainQuery.poHeader.po_currency_id,
                       po_currency = poCurrency.currency_code,
                       item_id = mainQuery.item.id,
                       item_no = mainQuery.item.item_no,
                       description_1 = mainQuery.item.description_1,
                       description_2 = mainQuery.item.description_2,
                       primary_uom = mainQuery.item.primary_uom,
                       secondary_uom = mainQuery.item.secondary_uom,
                       supplier_part_no = mainQuery.supplier_part_no,
                       supplier_material_code = mainQuery.supplier_material_code,
                       supplier_material_description = mainQuery.supplier_material_description,
                       ship_to_branch_plant_no = shipToBranchPlant.branch_plant_no,
                       ship_to_branch_plant_name = shipToBranchPlant.branch_plant_name,
                       unit_cost = mainQuery.poDetail.unit_cost,
                       unit_discount = mainQuery.poDetail.unit_discount,
                       secondary_supplier_no = secondarySupplier.sec_supplier_no,
                       secondary_supplier_name = secondarySupplier.sec_supplier_name,
                       instruction_to_supplier = mainQuery.poDetail.instruction_to_supplier,
                       internal_reference = mainQuery.poDetail.internal_reference,
                       notes_to_warehouse = mainQuery.poDetail.notes_to_warehouse,
                       supplier_acknowledgement_no = mainQuery.poDetail.supplier_acknowledgement_no,
                       requested_date = mainQuery.poDetail.requested_date,
                       quoted_ex_fac_date_earliest = mainQuery.poDetail.quoted_ex_fac_date_earliest,
                       quoted_ex_fac_date_latest = mainQuery.poDetail.quoted_ex_fac_date_latest,
                       ack_ex_fac_date = mainQuery.poDetail.ack_ex_fac_date,
                       forecast_ex_wh_date = mainQuery.poDetail.ack_ex_fac_date,
                       collection_date_time = mainQuery.poDetail.collection_date_time,
                       extended_cost = mainQuery.poDetail.extended_cost,
                       lot_label_required_flag = mainQuery.lot_label_required_flag,
                       created_on = mainQuery.poDetail.created_on,
                       created_by = mainQuery.poDetail.created_by,
                       last_modified_on = mainQuery.poDetail.last_modified_on,
                       last_modified_by = mainQuery.poDetail.last_modified_by
                   };
        }

        public async Task<List<PODetail>> GetPoLineForDeleteAsync(int poHeaderId, List<int> poDetailIDs)
        {
            return await _dbContext.PoDetails
            .Where(x => x.po_header_id == poHeaderId && poDetailIDs.Contains(x.id))
                .ToListAsync();
        }

        public async Task<List<PoNoListResponseDetail>> GetPoNoInfoList(PoNoRequestDetail request)
        {
            var poHeaderQuery = _dbContext.PoHeaders.AsNoTracking();

            if (request.Status is not null)
            {
                poHeaderQuery = poHeaderQuery.Where(x => request.Status.Contains(x.status_flag));
            }

            if (request.OnlyWithOpenQty)
            {
                var poHeaderIds = await GetPoHeaderWithOpenQtyAsync(request.Status);
                poHeaderQuery = poHeaderQuery.Where(x => poHeaderIds.Contains(x.id));
            }

            if (request.Suppliers is not null && request.Suppliers.Count > 0)
            {
                poHeaderQuery = poHeaderQuery.Where(x => request.Suppliers.Contains(x.supplier_id));
            }

            if (!string.IsNullOrEmpty(request.BpNo))
            {
                var branchPlantId = _dbContext.BranchPlant.AsNoTracking()
                    .FirstOrDefault(x => x.branch_plant_no.Equals(request.BpNo))?.id;

                if (branchPlantId.HasValue)
                {
                    poHeaderQuery = poHeaderQuery.Where(x => x.branch_plant_id == branchPlantId);
                }
            }

            var result = await poHeaderQuery.Select(x => new PoNoListResponseDetail
            {
                po_header_id = x.id,
                po_no = x.po_no,
                po_currency_id = x.po_currency_id
            }).ToListAsync();

            return result;
        }
        public async Task<bool> IsPoDetailWithStatusNotNew(int poHeaderId)
        {
            return (_dbContext.PoDetails.Where(x => x.po_header_id == poHeaderId
            && x.status_flag != PurchaseOrderDetail.StatusFlag.New
            && x.status_flag != PurchaseOrderDetail.StatusFlag.Cancelled).Count()) > 0;
        }
    }
}
