using Microsoft.EntityFrameworkCore;
using SERP.Application.Transactions.InboundShipments.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Transactions.Containers.Model;
using SERP.Domain.Transactions.InboundShipments;
using SERP.Domain.Transactions.InboundShipments.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Infrastructure.Transactions.InboundShipments
{
    internal class InboundShipmentRepository : GenericRepository<InboundShipment>, IInboundShipmentRepository
    {
        public InboundShipmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckExisted(int requestInboundShipmentId)
        {
            return await _dbContext.InboundShipments.AnyAsync(x => x.id == requestInboundShipmentId);
        }

        public async Task<bool> IsValidInboundShipment(int inboundShipmentId)
        {
            return await _dbContext.InboundShipments.AnyAsync(x =>
                x.id == inboundShipmentId
                && x.status_flag == DomainConstant.InboundShipments.StatusFlag.New);
        }

        public async Task<int[]> CheckInvalidInboundShipment(HashSet<int> inboundShipmentIds)
        {
            var ishIDs = await (from inboundShipment in _dbContext.InboundShipments
                                where inboundShipmentIds.Contains(inboundShipment.id)
                                select inboundShipment.id).ToArrayAsync();
            var result = inboundShipmentIds.Except(ishIDs);
            return result.ToArray();
        }

        public IQueryable<PagedIshResponseDetail> BuildISHFilterQuery(PagedFilterIsRequestModel request, out int totalRows)
        {
            var ishQuery = _dbContext.InboundShipments.AsNoTracking();
            var asnHeaderQuery = _dbContext.AsnHeaders.AsNoTracking();
            if (request.BranchPlants is not null && request.BranchPlants.Count > 0)
            {
                //ishQuery = ishQuery.Where(x => request.BranchPlants.Contains(x.branch_plant_id));
                asnHeaderQuery = asnHeaderQuery.Where(x => request.BranchPlants.Contains(x.branch_plant_id));
            }

            // "incoterms_list" : 
            if (request.incoterms is not null && request.incoterms.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.incoterms.Contains(x.incoterm));
            }
            // "country_of_loading_list" : 
            if (request.country_of_loading_list is not null && request.country_of_loading_list.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.country_of_loading_list.Contains(x.country_of_loading_id));
            }
            // "port_of_loading_list" : 
            if (request.port_of_loading_list is not null && request.port_of_loading_list.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.port_of_loading_list.Contains(x.port_of_loading_id));
            }
            // "country_of_discharge_list" : 
            if (request.country_of_discharge_list is not null && request.country_of_discharge_list.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.country_of_discharge_list.Contains(x.country_of_discharge_id));
            }
            // "port_of_discharge_list" : 
            if (request.port_of_discharge_list is not null && request.port_of_discharge_list.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.port_of_discharge_list.Contains(x.port_of_discharge_id));
            }
            // "status_List" : 
            if (request.status_list is not null && request.status_list.Count > 0)
            {
                ishQuery = ishQuery.Where(x => request.status_list.Contains(x.status_flag));
            }

            var asnQuery = from asnHeader in asnHeaderQuery
                           join shipToBranch in _dbContext.BranchPlant.AsNoTracking() on asnHeader.ship_to_branch_plant_id equals shipToBranch.id
                           join supplier in _dbContext.Supplier.AsNoTracking() on asnHeader.supplier_id equals supplier.id
                           join ishAsn in _dbContext.InboundShipmentASN.AsNoTracking() on asnHeader.id equals ishAsn.asn_header_id
                           //where asnHeader.inbound_shipment_id.HasValue
                           select new InboundShipmentPageAnsDetail
                           {
                               id = asnHeader.id,
                               asn_no = asnHeader.asn_no,
                               supplier_no = supplier.supplier_no,
                               supplier_name = supplier.supplier_name,
                               //shipment_arranged_supplier_flag = asnHeader.shipment_arranged_supplier_flag,
                               inbound_shipment_id = ishAsn.inbound_shipment_id,
                               ship_to_branch_plant_no = shipToBranch.branch_plant_no,
                               ship_to_branch_plant_name = shipToBranch.branch_plant_name
                           };

            // - Keywords are Inbound Shipment No, ASN No, Supplier No and Supplier Name
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                asnQuery = asnQuery.Where(x => x.asn_no.Contains(request.Keyword)
                                               || x.supplier_no.Contains(request.Keyword)
                                               || x.supplier_name.Contains(request.Keyword));

                var ishIds = asnQuery.Select(x => x.inbound_shipment_id!.Value).Distinct().ToList();
                ishQuery = ishQuery.Where(x => ishIds.Contains(x.id) || x.inbound_shipment_no.Contains(request.Keyword));
            }

            totalRows = ishQuery.Count();

            var query = from ish in ishQuery
                        join portOfLanding in _dbContext.Ports.AsNoTracking() on ish.port_of_loading_id equals portOfLanding.id into portOfLandingGroup
                        from portOfLanding in portOfLandingGroup.DefaultIfEmpty()

                        join portOfDischarge in _dbContext.Ports.AsNoTracking() on ish.port_of_discharge_id equals portOfDischarge.id into portOfDischargeGroup
                        from portOfDischarge in portOfDischargeGroup.DefaultIfEmpty()

                        join countryOfLoading in _dbContext.Country.AsNoTracking() on ish.country_of_loading_id equals countryOfLoading.id into countryOfLoadingGroup
                        from countryOfLoading in countryOfLoadingGroup.DefaultIfEmpty()

                        join countryOfDischarge in _dbContext.Country.AsNoTracking() on ish.country_of_discharge_id equals countryOfDischarge.id into countryOfDischargeGroup
                        from countryOfDischarge in countryOfDischargeGroup.DefaultIfEmpty()

                        join forwarder in _dbContext.Agent.AsNoTracking() on ish.forwarder_agent_id equals forwarder.id into forwarderGroup
                        from forwarderAgent in forwarderGroup.DefaultIfEmpty()

                        join forwarderCurrency in _dbContext.Currency.AsNoTracking() on ish.forwarder_invoice_currency_id equals forwarderCurrency.id into forwarderCurrencyGroup
                        from forwarderCurrency in forwarderCurrencyGroup.DefaultIfEmpty()

                        join shipping in _dbContext.Agent.AsNoTracking() on ish.shipping_agent_id equals shipping.id into shippingGroup
                        from shippingAgent in shippingGroup.DefaultIfEmpty()

                        join shippingCurrency in _dbContext.Currency.AsNoTracking() on ish.shipping_invoice_currency_id equals shippingCurrency.id into shippingCurrencyGroup
                        from shippingCurrency in shippingCurrencyGroup.DefaultIfEmpty()

                        join insurance in _dbContext.Agent.AsNoTracking() on ish.insurance_agent_id equals insurance.id into insuranceGroup
                        from insurance in insuranceGroup.DefaultIfEmpty()
                        select new PagedIshResponseDetail
                        {
                            inbound_shipment = new InboundShipmentDetail
                            {
                                id = ish.id,
                                status_flag = ish.status_flag,
                                inbound_shipment_no = ish.inbound_shipment_no,
                                freight_method = ish.freight_method,
                                incoterm = ish.incoterm,
                                port_of_loading_etd = ish.port_of_loading_etd,
                                port_of_discharge_eta = ish.port_of_discharge_eta,
                                country_of_loading_id = ish.country_of_loading_id,
                                country_of_loading_name = countryOfLoading.country_name,
                                port_of_loading_id = ish.port_of_loading_id,
                                port_of_loading_name = portOfLanding.port_name,
                                country_of_discharge_id = ish.country_of_discharge_id,
                                country_of_discharge_name = countryOfDischarge.country_name,
                                port_of_discharge_id = ish.port_of_discharge_id,
                                port_of_discharge_name = portOfDischarge.port_name,
                                vessel_flight_no = ish.vessel_flight_no,
                                connecting_vessel_flight_no = ish.connecting_vessel_flight_no,
                                notice_of_arrival_date = ish.notice_of_arrival_date,
                                import_permit_no = ish.import_permit_no,
                                internal_remarks = ish.internal_remarks,
                                forwarder_agent_id = ish.forwarder_agent_id,
                                forwarder_no = forwarderAgent.agent_no,
                                forwarder_name = forwarderAgent.agent_name,
                                forwarder_invoice_currency = forwarderCurrency.currency_code,
                                forwarder_invoice_no = ish.forwarder_invoice_no,
                                forwarder_invoice_amt = ish.forwarder_invoice_amt,
                                shipping_agent_id = ish.shipping_agent_id,
                                shipping_agent_no = shippingAgent.agent_no,
                                shipping_agent_name = shippingAgent.agent_name,
                                shipping_invoice_currency = shippingCurrency.currency_code,
                                shipping_invoice_no = ish.shipping_invoice_no,
                                shipping_invoice_amt = ish.shipping_invoice_amt,
                                insurance_agent_id = ish.insurance_agent_id,
                                insurance_agent_no = insurance.agent_no,
                                insurance_agent_name = insurance.agent_name,
                                created_on = ish.created_on,
                                created_by = ish.created_by,
                                last_modified_on = ish.last_modified_on,
                                last_modified_by = ish.last_modified_by
                            },
                            asns = asnQuery.Where(x => x.inbound_shipment_id == ish.id).ToList(),
                            bl_awbs = (from isBlAwb in _dbContext.InboundShipmentBLAWBs.AsNoTracking()
                                       where isBlAwb.inbound_shipment_id == ish.id
                                       select new InboundShipmentPageBlAwbDetail
                                       {
                                           bl_awb_no = isBlAwb.bl_awb_no,
                                           //containers = (from container in _dbContext.Containers.AsNoTracking()
                                           //              where container.inbound_shipment_blawb_id == isBlAwb.id
                                           //              select new ContainerDetail
                                           //              {
                                           //                  id = container.id,
                                           //                  status_flag = container.status_flag,
                                           //                  container_no = container.container_no,
                                           //                  shipment_type = container.shipment_type,
                                           //                  weight = container.weight
                                           //              }).ToList()
                                       }).ToList()
                        };

            return query;
        }

        public async Task<List<InboundShipment>> GetListInboundShipmentByAsnHeaderAsync(int asnHeaderId)
        {
            var ishAsn = await _dbContext.InboundShipmentASN.AsNoTracking()
                .Where(x => x.asn_header_id == asnHeaderId)
                .Select(x => x.inbound_shipment_id).ToListAsync();

            return await _dbContext.InboundShipments.AsNoTracking()
                .Where(x => ishAsn.Contains(x.id))
                .ToListAsync();
        }

        public async Task<AsnInboundShipmentResponseDetail?> GetInboundShipmentByAsnHeaderAsync(int asnHeaderId, string blAwbProvided)
        {

            var response = new AsnInboundShipmentResponseDetail
            {
                bl_awb_provided = blAwbProvided
            };

            if (blAwbProvided == DomainConstant.AdvancedShipmentNotices.BlAwbProvider.NotAvailable)
            {
                return response;
            }

            switch (blAwbProvided)
            {
                case DomainConstant.AdvancedShipmentNotices.BlAwbProvider.Yes:
                    {
                        // bl_awb_provided: "Y"
                        // - Get information from InboundShipmentASN, InboundShipment
                        var res = await (from inboundShipment in _dbContext.InboundShipments.AsNoTracking()
                                         join inboundShipmentASN in _dbContext.InboundShipmentASN.AsNoTracking() on inboundShipment.id equals inboundShipmentASN.inbound_shipment_id
                                         where inboundShipmentASN.asn_header_id == asnHeaderId
                                         select new AsnInboundShipmentResponseDetail
                                         {
                                             freight_method = inboundShipment.freight_method,
                                             incoterm = inboundShipment.incoterm,
                                             country_of_loading_id = inboundShipment.country_of_loading_id,
                                             port_of_loading_id = inboundShipment.port_of_loading_id,
                                             country_of_discharge_id = inboundShipment.country_of_discharge_id,
                                             port_of_discharge_id = inboundShipment.port_of_discharge_id,
                                             //cargo_ready_date = inboundShipment.cargo_ready_date,
                                             //shipment_type = inboundShipment.shipment_type,
                                             //hs_code = inboundShipment.hs_code,
                                             //total_packages = inboundShipment.total_packages,
                                             //total_gross_weight = inboundShipment.total_gross_weight,
                                             //volume = inboundShipment.volume,
                                             //cargo_description = inboundShipment.cargo_description,
                                             //marking_cargo = inboundShipment.marking_cargo,
                                             //marking_bl = inboundShipment.marking_bl,
                                             //collection_address = inboundShipment.collection_address,
                                         }).FirstOrDefaultAsync();

                        if (res is null)
                        {
                            return response;
                        }

                        response.freight_method = res.freight_method;
                        response.incoterm = res.incoterm;
                        response.country_of_loading_id = res.country_of_loading_id;
                        response.port_of_loading_id = res.port_of_loading_id;
                        response.country_of_discharge_id = res.country_of_discharge_id;
                        response.port_of_discharge_id = res.port_of_discharge_id;
                        break;
                    }
                case DomainConstant.AdvancedShipmentNotices.BlAwbProvider.No:
                    {
                        // bl_awb_provided: "N"
                        // - Get information from InboundShipmentRequest
                        var res = await (from inboundShipmentRequest in _dbContext.InboundShipmentRequests.AsNoTracking()
                                         where inboundShipmentRequest.asn_header_id == asnHeaderId
                                         select new AsnInboundShipmentResponseDetail
                                         {
                                             freight_method = inboundShipmentRequest.freight_method,
                                             incoterm = inboundShipmentRequest.incoterm,
                                             country_of_loading_id = inboundShipmentRequest.country_of_loading_id,
                                             port_of_loading_id = inboundShipmentRequest.port_of_loading_id,
                                             country_of_discharge_id = inboundShipmentRequest.country_of_discharge_id,
                                             port_of_discharge_id = inboundShipmentRequest.port_of_discharge_id,
                                             cargo_ready_date = inboundShipmentRequest.cargo_ready_date,
                                             shipment_type = inboundShipmentRequest.shipment_type,
                                             hs_code = inboundShipmentRequest.hs_code,
                                             total_packages = inboundShipmentRequest.total_packages,
                                             total_gross_weight = inboundShipmentRequest.total_gross_weight,
                                             volume = inboundShipmentRequest.volume,
                                             cargo_description = inboundShipmentRequest.cargo_description,
                                             marking_cargo = inboundShipmentRequest.marking_cargo,
                                             marking_bl = inboundShipmentRequest.marking_bl,
                                             collection_address = inboundShipmentRequest.collection_address,
                                             inbound_shipment_request_group_no = inboundShipmentRequest.inbound_shipment_request_group_no
                                         }).FirstOrDefaultAsync();

                        if (res is null)
                        {
                            return response;
                        }

                        response.freight_method = res.freight_method;
                        response.incoterm = res.incoterm;
                        response.country_of_loading_id = res.country_of_loading_id;
                        response.port_of_loading_id = res.port_of_loading_id;
                        response.country_of_discharge_id = res.country_of_discharge_id;
                        response.port_of_discharge_id = res.port_of_discharge_id;
                        response.cargo_ready_date = res.cargo_ready_date;
                        response.shipment_type = res.shipment_type;
                        response.hs_code = res.hs_code;
                        response.total_packages = res.total_packages;
                        response.total_gross_weight = res.total_gross_weight;
                        response.volume = res.volume;
                        response.cargo_description = res.cargo_description;
                        response.marking_cargo = res.marking_cargo;
                        response.marking_bl = res.marking_bl;
                        response.collection_address = res.collection_address;
                        response.inbound_shipment_request_group_no = res.inbound_shipment_request_group_no;
                        break;
                    }
            }

            return response;
        }
    }
}
