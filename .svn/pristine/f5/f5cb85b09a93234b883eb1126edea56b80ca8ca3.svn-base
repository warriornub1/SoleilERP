using AutoMapper;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.BranchPlants.Interfaces;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces;
using SERP.Application.Transactions.AdvancedShipmentNotices.Services;
using SERP.Application.Transactions.FilesTracking.Services;
using SERP.Application.Transactions.PurchaseOrders.DTOs.Request;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Application.Transactions.PurchaseOrders.Services;
using SERP.Application.Transactions.Receiving.DTOs.Request;
using SERP.Application.Transactions.Receiving.DTOs.Response;
using SERP.Application.Transactions.Receiving.Interfaces;
using SERP.Application.Transactions.SequencesTracking.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;
using SERP.Domain.Masters.Items;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.PurchaseOrders;
using SERP.Domain.Transactions.Receiving;
using SERP.Domain.Transactions.Receiving.Model;
using System.Linq.Expressions;
using static SERP.Application.Transactions.Receiving.DTOs.Request.CreateReceivingRequestDto;
using static SERP.Application.Transactions.Receiving.DTOs.Request.UpdateReceivingByActionDto;
using static SERP.Application.Transactions.Receiving.DTOs.Request.UpdateReceivingRequestDto;
using static SERP.Domain.Common.Constants.DomainConstant;
namespace SERP.Application.Transactions.Receiving.Services
{
    internal class ReceivingService : IReceivingService
    {
        private readonly ISequenceTrackingRepository _sequenceTrackingRepository;
        private readonly IReceivingHeaderRepository _receivingHeaderRepository;
        private readonly IReceivingDetailRepository _receivingDetailRepository;
        private readonly IReceivingFileRepository _receivingFileRepository;
        private readonly IFilesTrackingService _filesTrackingService;
        private readonly IBranchPlantRepository _branchPlantRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPOHeaderRepository _poHeaderRepository;
        private readonly IPODetailRepository _poDetailRepository;
        private readonly IASNHeaderRepository _asnHeaderRepository;
        private readonly IASNDetailRepository _asnDetailRepository;
        private readonly IItemUomConversionRepository _itemUomConversionRepository;
        private readonly IPurchaseOrderService _poService;
        private readonly IAdvancedShipmentNoticeService _asnService;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReceivingService(
            ISequenceTrackingRepository sequenceTrackingRepository,
            IReceivingHeaderRepository receivingHeaderRepository,
            IReceivingDetailRepository receivingDetailRepository,
            IReceivingFileRepository receivingFileRepository,
            IFilesTrackingService filesTrackingService,
            IBranchPlantRepository branchPlantRepository,
            ISupplierRepository supplierRepository,
            ICountryRepository countryRepository,
            IPOHeaderRepository poHeaderRepository,
            IPODetailRepository poDetailRepository,
            IASNDetailRepository asnDetailRepository,
            IASNHeaderRepository asnHeaderRepository,
            IItemUomConversionRepository itemUomConversionRepository, IItemRepository itemRepository,
            IPurchaseOrderService poService, IAdvancedShipmentNoticeService asnService,
            IUnitOfWork unitOfWork, IMapper mapper
        )
        {
            _sequenceTrackingRepository = sequenceTrackingRepository;
            _receivingHeaderRepository = receivingHeaderRepository;
            _receivingDetailRepository = receivingDetailRepository;
            _receivingFileRepository = receivingFileRepository;
            _filesTrackingService = filesTrackingService;
            _branchPlantRepository = branchPlantRepository;
            _supplierRepository = supplierRepository;
            _countryRepository = countryRepository;
            _poHeaderRepository = poHeaderRepository;
            _poDetailRepository = poDetailRepository;
            _asnHeaderRepository = asnHeaderRepository;
            _asnDetailRepository = asnDetailRepository;
            _itemUomConversionRepository = itemUomConversionRepository;
            _poService = poService;
            _asnService = asnService;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> CreateReceivingAsync(string userId, CreateReceivingRequestDto request)
        {
            var validator = new CreateReceivingRequestDtoValidator();
            var validatorResult = await validator.ValidateAsync(request);
            if (validatorResult.Errors.Any())
                throw new BadRequestException(validatorResult);

            //check branch_plant_id
            var branchPlantExist = await _branchPlantRepository.GetByIdAsync(x => x.id == request.branch_plant_id && x.status_flag == ApplicationConstant.StatusFlag.Enabled);
            if (branchPlantExist == null)
                throw new BadRequestException(ErrorCodes.BranchPlantNotFound, ErrorMessages.BranchPlantNotFound);

            //check supplier
            var supplierExist = await _supplierRepository.GetByIdAsync(x => x.id == request.supplier_id && x.status_flag == ApplicationConstant.StatusFlag.Enabled);
            if (supplierExist == null)
                throw new BadRequestException(ErrorCodes.SupplierNotFound, ErrorMessages.SupplierNotFound);

            //check asn header id
            if (request.asn_header_id != null)
            {
                var asnHeaderExist = await _asnHeaderRepository.GetByIdAsync(x => x.id == request.asn_header_id && x.status_flag == DomainConstant.AdvancedShipmentNotices.StatusFlag.New);
                if (asnHeaderExist == null)
                    throw new BadRequestException(ErrorCodes.AdvancedShipmentNoticeNotFound, ErrorMessages.AdvancedShipmentNoticeNotFound);
            }
            //check country of origin
            List<int?> countryList = request.receiving_details.Where(x => x.country_of_origin_id != null).Select(x => x.country_of_origin_id).Distinct().ToList();
            if (countryList.Count != 0)
            {
                var countryExist = await _countryRepository.GetDictionaryAsync(x => countryList.Contains(x.id));
                if (countryExist == null || countryExist.Count != countryList.Count)
                    throw new BadRequestException(ErrorCodes.CountryOfOriginNotFound, ErrorMessages.CountryOfOriginNotFound);
            }
            //group by po_detail_id / asn_detail_id and sum qty
            var detailGroup = (request.asn_header_id == null) ? request.receiving_details.GroupBy(x => x.po_detail_id).
                    Select(group => new
                    {
                        id = group.Key,
                        totalQty = group.Sum(x => x.qty)
                    }).ToList() : request.receiving_details.GroupBy(x => x.asn_detail_id).
                    Select(group => new
                    {
                        id = group.Key,
                        totalQty = group.Sum(x => x.qty)
                    }).ToList();

            if (request.asn_header_id == null) // create by PO
            {
                foreach (var detail in detailGroup)
                    await validatePoDetailForReceiving((int)detail.id, detail.totalQty);
            }
            else // create by ASN
            {
                //!!!! po_detail_id can more than 1 for 1 ASN no !!!!!!!
                foreach (var detail in detailGroup)
                    await validateASNDetailForReceiving((int)request.asn_header_id, (int)detail.id, detail.totalQty);
            }

            try
            {
                _unitOfWork.BeginTransaction();
                DateTime currentTimestamp = DateTime.Now;

                //create Receiving Header
                //If draft_flag is true >> ReceivingHeader.status_flag and ReceivingDetail.status_flag will be 01: Draft.
                //If draft_flag is false >> ReceivingHeader.status_flag and ReceivingDetail.status_flag will be 11:Received , received_on = current timestamp.

                ReceivingHeader header = new ReceivingHeader();
                header.branch_plant_id = request.branch_plant_id;
                header.supplier_id = request.supplier_id;
                //if (request.asn_header_id != null)
                //    header.asn_header_id = request.asn_header_id;

                header.status_flag = request.draft_flag ? RcvHeader.StatusFlag.Draft : RcvHeader.StatusFlag.Received;
                if (!request.draft_flag) header.received_on = currentTimestamp;

                var rcvHeaderId = await CreateReceivingHeader(userId, _unitOfWork, header);

                //create Receiving Detail for list
                List<ReceivingDetail> rcvDetailList = new List<ReceivingDetail>();
                List<PODetail> poDetailList = new List<PODetail>();
                List<ASNDetail> asnDetailList = new List<ASNDetail>();
                if (request.asn_header_id == null)
                    poDetailList = await _poDetailRepository.GetPoDetailList(detailGroup.Select(x => (int)x.id).ToList());
                else
                    asnDetailList = await _asnDetailRepository.GetASNDetailList((int)request.asn_header_id, detailGroup.Select(x => (int)x.id).ToList());
                int detailLineNo = 0;
                foreach (var detail in request.receiving_details)
                {
                    ReceivingDetail rcvDetail = new ReceivingDetail();
                    rcvDetail.receiving_header_id = rcvHeaderId;
                    rcvDetail.line_no = ++detailLineNo;
                    rcvDetail.status_flag = request.draft_flag ? RcvDetail.StatusFlag.Draft : RcvDetail.StatusFlag.Received;
                    rcvDetail.package_no = detail.package_no;
                    rcvDetail.country_of_origin_id = detail.country_of_origin_id;
                    rcvDetail.supplier_document_no = detail.supplier_document_no;
                    rcvDetail.supplier_document_type = detail.supplier_document_type;
                    rcvDetail.qty = detail.qty;
                    rcvDetail.po_detail_id = detail.po_detail_id;
                    if (request.asn_header_id == null)
                    {
                        var poItem = poDetailList.Where(x => x.id == detail.po_detail_id).Select(x => new { item_id = x.item_id, uom = x.uom }).FirstOrDefault();
                        rcvDetail.item_id = poItem.item_id;
                        rcvDetail.po_uom = poItem.uom;

                        rcvDetailList.Add(rcvDetail);
                    }
                    else
                    {
                        // rcvDetail.asn_detail_id = detail.asn_detail_id;
                        var asnItem = asnDetailList.Where(x => x.id == detail.asn_detail_id).Select(x => new { item_id = x.item_id, uom = x.uom }).FirstOrDefault();
                        rcvDetail.item_id = asnItem.item_id;
                        rcvDetail.po_uom = asnItem.uom;

                        rcvDetailList.Add(rcvDetail);
                    }
                }
                await CreateReceivingDetail(userId, _unitOfWork, rcvDetailList);

                //update PO/ASN detail
                //update PO/ASN header and detail status
                if (request.asn_header_id == null) // add by PO
                {
                    List<PODetail> dtlList = new List<PODetail>();
                    List<int> poHeaderIds = new List<int>();
                    foreach (var dtl in detailGroup)
                    {
                        PODetail poDtl = await _poDetailRepository.GetByIdAsync(x => x.id == dtl.id);
                        poDtl.open_qty = poDtl.open_qty - dtl.totalQty;
                        if (poDtl.status_flag != PurchaseOrderDetail.StatusFlag.InProcess)
                            poDtl.status_flag = PurchaseOrderDetail.StatusFlag.InProcess;
                        dtlList.Add(poDtl);
                        if (!poHeaderIds.Contains(poDtl.po_header_id)) poHeaderIds.Add(poDtl.po_header_id);
                    }
                    await _poDetailRepository.UpdateRangeAsync(dtlList);
                    await _unitOfWork.SaveChangesAsync();

                    foreach (int poHeaderId in poHeaderIds)
                    {
                        POHeader poHeader = await _poHeaderRepository.GetByIdAsync(x => x.id == poHeaderId);
                        if (poHeader.status_flag != PurchaseOrder.StatusFlag.InProcess)
                        {
                            poHeader.status_flag = PurchaseOrder.StatusFlag.InProcess;
                            await _poHeaderRepository.UpdateAsync(poHeader);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                }
                else // add by ASN
                {
                    List<ASNDetail> dtlList = new List<ASNDetail>();
                    //List<int> asnHeaderIds = new List<int>();
                    foreach (var dtl in detailGroup)
                    {
                        ASNDetail asnDtl = await _asnDetailRepository.GetByIdAsync(x => x.id == dtl.id);
                        if (asnDtl.qty < dtl.totalQty)
                            // deduct from po 



                            // asnDtl.received_qty = asnDtl.received_qty + dtl.totalQty;
                            //  if (asnDtl.status_flag != AdvancedShipmentNoticesDetail.StatusFlag.InProcess)
                            asnDtl.status_flag = AdvancedShipmentNoticesDetail.StatusFlag.Closed;
                        asnDtl.last_modified_by = userId;
                        asnDtl.last_modified_on = currentTimestamp;
                        dtlList.Add(asnDtl);
                        //  if (!asnHeaderIds.Contains(asnDtl.asn_header_id)) asnHeaderIds.Add(asnDtl.asn_header_id);
                    }
                    await _asnDetailRepository.UpdateRangeAsync(dtlList);
                    await _unitOfWork.SaveChangesAsync();

                    //foreach (int asnHeaderId in asnHeaderIds)
                    //{
                    ASNHeader asnHeader = await _asnHeaderRepository.GetByIdAsync(x => x.id == request.asn_header_id);
                    //if (asnHeader.status_flag != DomainConstant.AdvancedShipmentNotices.StatusFlag.InProcess)
                    //{
                    asnHeader.status_flag = DomainConstant.AdvancedShipmentNotices.StatusFlag.Closed;
                    asnHeader.last_modified_by = userId;
                    asnHeader.last_modified_on = currentTimestamp;
                    await _asnHeaderRepository.UpdateAsync(asnHeader);
                    await _unitOfWork.SaveChangesAsync();
                    //}
                    //}
                }
                _unitOfWork.Commit();
                return rcvHeaderId;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task UpdateReceivingAsync(string userId, UpdateReceivingRequestDto request)
        {
            var validator = new UpdateReceivingRequestDtoValidator();
            var validatorResult = await validator.ValidateAsync(request);
            if (validatorResult.Errors.Any())
                throw new BadRequestException(validatorResult);

            // get receiving header
            ReceivingHeader rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == request.receiving_header_id);
            if (rcvHeader == null)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            if (rcvHeader.status_flag != RcvHeader.StatusFlag.Draft && rcvHeader.status_flag != RcvHeader.StatusFlag.Received)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderStatusNotMatch, ErrorMessages.ReceivingHeaderStatusNotMatch);

            //check country of origin
            List<int?> countryList = request.receiving_details.Where(x => x.country_of_origin_id != null).Select(x => x.country_of_origin_id).Distinct().ToList();
            if (countryList.Count != 0)
            {
                var countryExist = await _countryRepository.GetDictionaryAsync(x => countryList.Contains(x.id));
                if (countryExist == null || countryExist.Count != countryList.Count)
                    throw new BadRequestException(ErrorCodes.CountryOfOriginNotFound, ErrorMessages.CountryOfOriginNotFound);
            }
            //get new added detail
            //group by po_detail_id / asn_detail_id and sum qty
            //var detailGroup = rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder ?
            //    request.receiving_details.Where(x => x.receiving_detail_id == null).GroupBy(x => x.po_detail_id).
            //        Select(group => new
            //        {
            //            id = group.Key,
            //            totalQty = group.Sum(x => x.po_qty)
            //        }).ToList()
            //        :
            //        request.receiving_details.Where(x => x.receiving_detail_id == null).GroupBy(x => x.asn_detail_id).
            //        Select(group => new
            //        {
            //            id = group.Key,
            //            totalQty = group.Sum(x => x.po_qty)
            //        }).ToList();
            var detailGroup =
                request.receiving_details.Where(x => x.receiving_detail_id == null).GroupBy(x => x.po_detail_id).
                    Select(group => new
                    {
                        id = group.Key,
                        totalQty = group.Sum(x => x.qty)
                    }).ToList();


            //get receiving detail
            List<int> rcvDetailIds = request.receiving_details.Where(x => x.receiving_detail_id != null).Select(x => (int)x.receiving_detail_id).ToList();
            List<ReceivingDetail> rcvDetailList = await _receivingDetailRepository.GetReceivingDetailListAsync(rcvDetailIds, rcvHeader.id);
            List<int> itemIds = rcvDetailList.Select(x => x.item_id).Distinct().ToList();
            List<ItemUomConversion> itemUomConversionList = await _itemUomConversionRepository.GetItemUomConversionListAsync(itemIds);

            foreach (var detail in request.receiving_details)
            {
                if (detail.receiving_detail_id != null)
                {
                    ReceivingDetail receivingDetail = rcvDetailList.Where(x => x.id == detail.receiving_detail_id).FirstOrDefault();
                    if (receivingDetail == null)
                        throw new BadRequestException(ErrorCodes.ReceivingDetailNotFound, ErrorMessages.ReceivingDetailNotFound);

                    int newRcvQty = detail.qty;
                    int oldRcvQty = receivingDetail.qty;
                    int oldRcvQtyInPoUom = receivingDetail.qty;
                    if (receivingDetail.uom != receivingDetail.po_uom)
                    {
                        ItemUomConversion itemUomConversion = itemUomConversionList.Where(x => x.item_id == receivingDetail.item_id).FirstOrDefault();
                        if (itemUomConversion == null)
                            throw new BadRequestException(ErrorCodes.ItemUomConversionNotFound, ErrorMessages.ItemUomConversionNotFound);

                        newRcvQty = (detail.qty / itemUomConversion.secondary_uom_qty) * itemUomConversion.primary_uom_qty; //convert to primary uom
                        oldRcvQtyInPoUom = receivingDetail.qty / itemUomConversion.primary_uom_qty * itemUomConversion.secondary_uom_qty;
                    }
                    if (newRcvQty != oldRcvQty) //compare in primary uom
                    {
                        if (newRcvQty < oldRcvQty && newRcvQty < receivingDetail.inspected_qty)
                            throw new BadRequestException(ErrorCodes.ReceivingQtyCannotLessThanInspectedQty, ErrorMessages.ReceivingQtyCannotLessThanInspectedQty);

                        int qtyChanged = newRcvQty - oldRcvQtyInPoUom;
                        //int? detailId = rcvHeader.receiving_from_document_type == RcvHeader.RcvFromDocType.PurchaseOrder ? receivingDetail.po_detail_id : receivingDetail.asn_detail_id;
                        int? detailId = receivingDetail.po_detail_id;

                        var dtl = detailGroup.Find(x => x.id == detailId);
                        if (dtl == null)
                            detailGroup.Add(new
                            {
                                id = detailId,
                                totalQty = qtyChanged

                            });
                        else
                        {
                            var newDtl = new
                            {
                                id = detailId,
                                totalQty = dtl.totalQty + qtyChanged
                            };
                            detailGroup.Remove(dtl);
                            detailGroup.Add(newDtl);
                        }
                    }
                }
            }

            //if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder)
            //{
            foreach (var detail in detailGroup)
                await validatePoDetailForReceiving((int)detail.id, detail.totalQty);
            //}
            //else if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.AdvancedShipmentNotice)
            //{
            //    foreach (var detail in detailGroup)
            //        await validateASNDetailForReceiving((int)detail.id, detail.totalQty);
            //}

            try
            {
                _unitOfWork.BeginTransaction();
                DateTime currentTimestamp = DateTime.Now;

                if (!request.draft_flag && rcvHeader.status_flag == RcvHeader.StatusFlag.Draft)
                {
                    rcvHeader.status_flag = RcvHeader.StatusFlag.Received;
                    rcvHeader.received_on = currentTimestamp;
                    rcvHeader.last_modified_by = userId;
                    rcvHeader.last_modified_on = currentTimestamp;
                    await _receivingHeaderRepository.UpdateAsync(rcvHeader);
                    await _unitOfWork.SaveChangesAsync();
                }

                //create Receiving Detail for list
                List<ReceivingDetail> newRcvDetailList = new List<ReceivingDetail>();
                List<ReceivingDetail> updateRcvDetailList = new List<ReceivingDetail>();

                List<PODetail> poDetailList = new List<PODetail>();
                List<ASNDetail> asnDetailList = new List<ASNDetail>();
                //if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder)
                poDetailList = await _poDetailRepository.GetPoDetailList(detailGroup.Select(x => (int)x.id).ToList());
                //else if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.AdvancedShipmentNotice)
                //    asnDetailList = await _asnDetailRepository.GetASNDetailList(detailGroup.Select(x => (int)x.id).ToList());
                int lastLineNo = await _receivingDetailRepository.GetReceivingDetailLastLineNoAsync(rcvHeader.id);
                foreach (var detail in request.receiving_details)
                {
                    if (detail.receiving_detail_id == null) //new added
                    {
                        ReceivingDetail rcvDetail = new ReceivingDetail();
                        rcvDetail.receiving_header_id = rcvHeader.id;
                        rcvDetail.line_no = ++lastLineNo;
                        rcvDetail.status_flag = request.draft_flag ? RcvDetail.StatusFlag.Draft : RcvDetail.StatusFlag.Received;
                        rcvDetail.package_no = detail.package_no;
                        rcvDetail.country_of_origin_id = detail.country_of_origin_id;
                        rcvDetail.supplier_document_no = detail.supplier_document_no;
                        rcvDetail.supplier_document_type = detail.supplier_document_type;
                        rcvDetail.qty = detail.qty;
                        //if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder)
                        //{
                        rcvDetail.po_detail_id = detail.po_detail_id;
                        var poItem = poDetailList.Where(x => x.id == detail.po_detail_id).Select(x => new { item_id = x.item_id, uom = x.uom }).FirstOrDefault();
                        rcvDetail.item_id = poItem.item_id;
                        rcvDetail.po_uom = poItem.uom;

                        newRcvDetailList.Add(rcvDetail);
                        //}
                        //else if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.AdvancedShipmentNotice)
                        //{
                        //    rcvDetail.asn_detail_id = detail.asn_detail_id;
                        //    var asnItem = asnDetailList.Where(x => x.id == detail.asn_detail_id).Select(x => new { item_id = x.item_id, uom = x.uom }).FirstOrDefault();
                        //    rcvDetail.item_id = asnItem.item_id;
                        //    rcvDetail.po_uom = asnItem.uom;

                        //    newRcvDetailList.Add(rcvDetail);
                        //}
                    }
                    else //update existing
                    {
                        ReceivingDetail receivingDetail = rcvDetailList.Where(x => x.id == detail.receiving_detail_id).FirstOrDefault();

                        int newRcvQty = detail.qty;
                        if (receivingDetail.uom != receivingDetail.po_uom)
                        {
                            ItemUomConversion itemUomConversion = itemUomConversionList.Where(x => x.item_id == receivingDetail.item_id).FirstOrDefault();
                            newRcvQty = (detail.qty / itemUomConversion.secondary_uom_qty) * itemUomConversion.primary_uom_qty; //convert to primary uom
                        }
                        receivingDetail.qty = newRcvQty;
                        if (detail.package_no != null)
                            receivingDetail.package_no = detail.package_no;
                        if (detail.country_of_origin_id != null)
                            receivingDetail.country_of_origin_id = detail.country_of_origin_id;
                        if (detail.supplier_document_no != null)
                            receivingDetail.supplier_document_no = detail.supplier_document_no;
                        if (detail.supplier_document_type != null)
                            receivingDetail.supplier_document_type = detail.supplier_document_type;

                        if (!request.draft_flag && receivingDetail.status_flag == RcvDetail.StatusFlag.Draft)
                            receivingDetail.status_flag = RcvDetail.StatusFlag.Received;
                        receivingDetail.last_modified_by = userId;
                        receivingDetail.last_modified_on = currentTimestamp;

                        updateRcvDetailList.Add(receivingDetail);
                    }
                }
                if (newRcvDetailList.Count > 0)
                    await CreateReceivingDetail(userId, _unitOfWork, newRcvDetailList);
                if (updateRcvDetailList.Count > 0)
                {
                    await _receivingDetailRepository.UpdateRangeAsync(updateRcvDetailList);
                    await _unitOfWork.SaveChangesAsync();
                }

                //update PO/ASN detail
                //update PO/ASN header and detail status
                //if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.PurchaseOrder)
                //{
                List<PODetail> dtlList = new List<PODetail>();
                List<int> poHeaderIds = new List<int>();
                foreach (var dtl in detailGroup)
                {
                    PODetail poDtl = await _poDetailRepository.GetByIdAsync(x => x.id == dtl.id);
                    poDtl.open_qty = poDtl.open_qty - dtl.totalQty;
                    if (poDtl.status_flag == PurchaseOrderDetail.StatusFlag.New)
                        poDtl.status_flag = PurchaseOrderDetail.StatusFlag.InProcess;
                    dtlList.Add(poDtl);
                    if (!poHeaderIds.Contains(poDtl.po_header_id)) poHeaderIds.Add(poDtl.po_header_id);
                }
                await _poDetailRepository.UpdateRangeAsync(dtlList);
                await _unitOfWork.SaveChangesAsync();

                foreach (int poHeaderId in poHeaderIds)
                {
                    POHeader poHeader = await _poHeaderRepository.GetByIdAsync(x => x.id == poHeaderId);
                    if (poHeader.status_flag == PurchaseOrder.StatusFlag.New)
                    {
                        poHeader.status_flag = PurchaseOrder.StatusFlag.InProcess;
                        await _poHeaderRepository.UpdateAsync(poHeader);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
                //}
                //else if (rcvHeader.receiving_from_document_type == ApplicationConstant.ReceivedFromDocType.AdvancedShipmentNotice)
                //{
                //    List<ASNDetail> dtlList = new List<ASNDetail>();
                //    List<int> asnHeaderIds = new List<int>();
                //    foreach (var dtl in detailGroup)
                //    {
                //        ASNDetail asnDtl = await _asnDetailRepository.GetByIdAsync(x => x.id == dtl.id);
                //        asnDtl.received_qty = asnDtl.received_qty + dtl.totalQty;
                //        if (asnDtl.status_flag == AdvancedShipmentNoticesDetail.StatusFlag.New)
                //            asnDtl.status_flag = AdvancedShipmentNoticesDetail.StatusFlag.InProcess;
                //        dtlList.Add(asnDtl);
                //        if (!asnHeaderIds.Contains(asnDtl.asn_header_id)) asnHeaderIds.Add(asnDtl.asn_header_id);
                //    }
                //    await _asnDetailRepository.UpdateRangeAsync(dtlList);
                //    await _unitOfWork.SaveChangesAsync();

                //    foreach (int asnHeaderId in asnHeaderIds)
                //    {
                //        ASNHeader asnHeader = await _asnHeaderRepository.GetByIdAsync(x => x.id == asnHeaderId);
                //        if (asnHeader.status_flag == DomainConstant.AdvancedShipmentNotices.StatusFlag.New)
                //        {
                //            asnHeader.status_flag = DomainConstant.AdvancedShipmentNotices.StatusFlag.InProcess;
                //            await _asnHeaderRepository.UpdateAsync(asnHeader);
                //            await _unitOfWork.SaveChangesAsync();
                //        }
                //    }
                //}

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task DeleteReceivingLineAsync(string userId, int receivingLineId)
        {
            var receivingDetail = await _receivingDetailRepository.GetByIdAsync(x => x.id == receivingLineId);
            if (receivingDetail == null)
                throw new BadRequestException(ErrorCodes.ReceivingDetailNotFound, ErrorMessages.ReceivingDetailNotFound);

            if (receivingDetail.status_flag == RcvDetail.StatusFlag.Cancelled)
                throw new BadRequestException(ErrorCodes.ReceivingDetailCancelled, ErrorMessages.ReceivingDetailCancelled);

            if (receivingDetail.status_flag != RcvDetail.StatusFlag.Draft && receivingDetail.status_flag != RcvDetail.StatusFlag.Received)
                throw new BadRequestException(ErrorCodes.ReceivingDetailCannotBeDeleted, ErrorMessages.ReceivingDetailCannotBeDeleted);

            var receivingHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == receivingDetail.receiving_header_id);

            try
            {
                _unitOfWork.BeginTransaction();
                DateTime currentTimestamp = DateTime.Now;
                int receivingQty = receivingDetail.qty;

                //convert receiving qty to po_uom
                if (receivingDetail.uom != receivingDetail.po_uom)
                {
                    var itemUomConversion = await _itemUomConversionRepository.GetByIdAsync(x => x.item_id == receivingDetail.item_id);
                    if (itemUomConversion == null)
                        throw new BadRequestException(ErrorCodes.ItemUomConversionNotFound, ErrorMessages.ItemUomConversionNotFound);

                    receivingQty = receivingDetail.qty / itemUomConversion.primary_uom_qty * itemUomConversion.secondary_uom_qty;
                }
                //update / delete Receiving Detail 
                if (receivingDetail.status_flag != RcvDetail.StatusFlag.Draft)
                {
                    await _receivingDetailRepository.DeleteAsync(receivingDetail);
                    await _unitOfWork.SaveChangesAsync();
                }
                else if (receivingDetail.status_flag != RcvDetail.StatusFlag.Received)
                {
                    receivingDetail.status_flag = RcvDetail.StatusFlag.Cancelled;
                    receivingDetail.last_modified_by = userId;
                    receivingDetail.last_modified_on = currentTimestamp;
                    await _receivingDetailRepository.UpdateAsync(receivingDetail);
                    await _unitOfWork.SaveChangesAsync();
                }
                //revert back qty to PO /Asn
                //if (receivingHeader.receiving_from_document_type == RcvHeader.RcvFromDocType.PurchaseOrder)
                //{
                bool toCheckPoHeaderStatus = false;
                var podetail = await _poDetailRepository.GetByIdAsync(x => x.id == receivingDetail.po_detail_id);
                if (podetail == null)
                    throw new BadRequestException(ErrorCodes.PODetailNotFound, ErrorMessages.PODetailNotFound);

                int newOpenQty = podetail.open_qty + receivingQty;
                podetail.open_qty = newOpenQty;
                if (podetail.qty == newOpenQty)
                {
                    podetail.status_flag = PurchaseOrderDetail.StatusFlag.New;
                    toCheckPoHeaderStatus = true;
                }
                podetail.last_modified_by = userId;
                podetail.last_modified_on = currentTimestamp;
                await _poDetailRepository.UpdateAsync(podetail);
                await _unitOfWork.SaveChangesAsync();

                if (toCheckPoHeaderStatus)
                    await _poService.UpdatePoHeaderStatusToNewByPODetailStatus(userId, _unitOfWork, podetail.po_header_id);
                //}
                //else if (receivingHeader.receiving_from_document_type == RcvHeader.RcvFromDocType.AdvancedShipmentNotice)
                //{
                //    bool toCheckAsnHeaderStatus = false;
                //    var asndetail = await _asnDetailRepository.GetByIdAsync(x => x.id == receivingDetail.asn_detail_id);
                //    if (asndetail == null)
                //        throw new BadRequestException(ErrorCodes.ASNDetailNotFound, ErrorMessages.ASNDetailNotFound);

                //    int newReceivedQty = asndetail.received_qty - receivingQty;
                //    asndetail.received_qty = newReceivedQty;
                //    if (newReceivedQty == 0)
                //    {
                //        asndetail.status_flag = AdvancedShipmentNoticesDetail.StatusFlag.New;
                //        toCheckAsnHeaderStatus = true;
                //    }
                //    asndetail.last_modified_by = userId;
                //    asndetail.last_modified_on = currentTimestamp;
                //    await _asnDetailRepository.UpdateAsync(asndetail);
                //    await _unitOfWork.SaveChangesAsync();

                //    if (toCheckAsnHeaderStatus)
                //        await _poService.UpdatePoHeaderStatusToNewByPODetailStatus(userId, _unitOfWork, asndetail.asn_header_id);
                //}
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task UpdateByActionAsync(string userId, UpdateReceivingByActionDto request)
        {
            var validator = new UpdateReceivingByActionDtoValidator();
            var validatorResult = await validator.ValidateAsync(request);
            if (validatorResult.Errors.Any())
                throw new BadRequestException(validatorResult);

            // get receiving header
            ReceivingHeader rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == request.receiving_header_id);
            if (rcvHeader == null)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            if (request.action == ApplicationConstant.ReceivingUpdateAction.AssignInspector
                && rcvHeader.status_flag != RcvHeader.StatusFlag.Received
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectorAssigned
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectionInProgress
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectionForCorrection)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderStatusNotMatch, ErrorMessages.ReceivingHeaderStatusNotMatch);
            else if (request.action == ApplicationConstant.ReceivingUpdateAction.UnassignInspector
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectorAssigned
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectionInProgress
                && rcvHeader.status_flag != RcvHeader.StatusFlag.InspectionForCorrection)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderStatusNotMatch, ErrorMessages.ReceivingHeaderStatusNotMatch);

            DateTime currentTimestamp = DateTime.Now;
            if (request.action == ApplicationConstant.ReceivingUpdateAction.AssignInspector)
            {
                if (rcvHeader.status_flag == RcvHeader.StatusFlag.Received)
                    rcvHeader.status_flag = RcvHeader.StatusFlag.InspectorAssigned;
                rcvHeader.inspected_by = request.inspected_by;
            }
            else if (request.action == ApplicationConstant.ReceivingUpdateAction.UnassignInspector)
            {
                if (rcvHeader.status_flag == RcvHeader.StatusFlag.InspectorAssigned)
                    rcvHeader.status_flag = RcvHeader.StatusFlag.Received;
                rcvHeader.inspected_by = null;
            }
            else if (request.action == ApplicationConstant.ReceivingUpdateAction.InspectionStart)
            {
                rcvHeader.status_flag = RcvHeader.StatusFlag.InspectionInProgress;
                rcvHeader.inspected_by = null;
                rcvHeader.inspection_start_on = currentTimestamp;
            }
            rcvHeader.last_modified_by = userId;
            rcvHeader.last_modified_on = currentTimestamp;
            await _receivingHeaderRepository.UpdateAsync(rcvHeader);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteReceivingAsync(string userId, int receivingHeaderId)
        {
            var rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == receivingHeaderId);
            if (rcvHeader == null)
                throw new BadRequestException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            if (rcvHeader.status_flag != RcvHeader.StatusFlag.Draft && rcvHeader.status_flag != RcvHeader.StatusFlag.Received)
                throw new BadRequestException(ErrorCodes.ReceivingCannotBeDeleted, ErrorMessages.ReceivingCannotBeDeleted);

            List<ReceivingDetail> rcvDetailList = await _receivingDetailRepository.GetDetailByReceivingHeaderIdAsync(receivingHeaderId);

            if (rcvHeader.status_flag != RcvHeader.StatusFlag.Draft)
            {
                int notDraftDetail = rcvDetailList.Where(x => x.status_flag != RcvDetail.StatusFlag.Draft).Count();
                if (notDraftDetail > 0)
                    throw new BadRequestException(ErrorCodes.ReceivingCannotBeDeleted, ErrorMessages.ReceivingCannotBeDeleted);
            }
            else if (rcvHeader.status_flag != RcvHeader.StatusFlag.Received)
            {
                int notReceivingCancelledDetail = rcvDetailList
                    .Where(x => x.status_flag != RcvDetail.StatusFlag.Received
                        && x.status_flag != RcvDetail.StatusFlag.Cancelled).Count();
                if (notReceivingCancelledDetail > 0)
                    throw new BadRequestException(ErrorCodes.ReceivingCannotBeDeleted, ErrorMessages.ReceivingCannotBeDeleted);
            }

            try
            {
                _unitOfWork.BeginTransaction();
                DateTime currentTimestamp = DateTime.Now;

                List<int> itemList = rcvDetailList.Where(x => x.uom != x.po_uom).Select(x => x.item_id).Distinct().ToList();
                List<ItemUomConversion> conversionList = await _itemUomConversionRepository.GetItemUomConversionListAsync(itemList);
                foreach (var detail in rcvDetailList)
                {
                    if (detail.status_flag == RcvDetail.StatusFlag.Cancelled)
                        continue;

                    int receivingQty = detail.qty;
                    //convert receiving qty to po_uom
                    if (detail.uom != detail.po_uom)
                    {
                        var itemUomConversion = conversionList.Where(x => x.item_id == detail.item_id).FirstOrDefault();
                        if (itemUomConversion == null)
                            throw new BadRequestException(ErrorCodes.ItemUomConversionNotFound, ErrorMessages.ItemUomConversionNotFound);

                        receivingQty = detail.qty / itemUomConversion.primary_uom_qty * itemUomConversion.secondary_uom_qty;
                    }
                    //update / delete Receiving Detail 
                    if (rcvHeader.status_flag == RcvHeader.StatusFlag.Draft)
                    {
                        await _receivingDetailRepository.DeleteAsync(detail);
                        await _unitOfWork.SaveChangesAsync();
                    }
                    else if (rcvHeader.status_flag == RcvHeader.StatusFlag.Received)
                    {
                        detail.status_flag = RcvDetail.StatusFlag.Cancelled;
                        detail.last_modified_by = userId;
                        detail.last_modified_on = currentTimestamp;
                        await _receivingDetailRepository.UpdateAsync(detail);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    bool toCheckPoHeaderStatus = false;
                    var podetail = await _poDetailRepository.GetByIdAsync(x => x.id == detail.po_detail_id);
                    if (podetail == null)
                        throw new BadRequestException(ErrorCodes.PODetailNotFound, ErrorMessages.PODetailNotFound);

                    int newOpenQty = podetail.open_qty + receivingQty;
                    podetail.open_qty = newOpenQty;
                    if (podetail.qty == newOpenQty)
                    {
                        podetail.status_flag = PurchaseOrderDetail.StatusFlag.New;
                        toCheckPoHeaderStatus = true;
                    }
                    podetail.last_modified_by = userId;
                    podetail.last_modified_on = currentTimestamp;
                    await _poDetailRepository.UpdateAsync(podetail);
                    await _unitOfWork.SaveChangesAsync();
                    if (toCheckPoHeaderStatus)
                        await _poService.UpdatePoHeaderStatusToNewByPODetailStatus(userId, _unitOfWork, podetail.po_header_id);
                }

                //update / delete Receiving Header 
                if (rcvHeader.status_flag == RcvHeader.StatusFlag.Draft)
                {
                    await _receivingHeaderRepository.DeleteAsync(rcvHeader);
                    await _unitOfWork.SaveChangesAsync();
                }
                else if (rcvHeader.status_flag == RcvHeader.StatusFlag.Received)
                {
                    rcvHeader.status_flag = RcvHeader.StatusFlag.Cancelled;
                    rcvHeader.last_modified_by = userId;
                    rcvHeader.last_modified_on = currentTimestamp;
                    await _receivingHeaderRepository.UpdateAsync(rcvHeader);
                    await _unitOfWork.SaveChangesAsync();
                }
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task UploadFileAsync(string userId, int receivingHeaderId, int receivingDetailId, List<FileInfoRequestDto> fileList)
        {
            var rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == receivingHeaderId && x.status_flag != RcvHeader.StatusFlag.Cancelled);

            if (rcvHeader == null)
                throw new NotFoundException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            var rcvDetail = await _receivingDetailRepository.GetByIdAsync(x => x.id == receivingDetailId && x.status_flag != RcvDetail.StatusFlag.Cancelled);

            if (rcvDetail == null)
                throw new NotFoundException(ErrorCodes.ReceivingDetailNotFound, ErrorMessages.ReceivingDetailNotFound);

            try
            {
                _unitOfWork.BeginTransaction();

                foreach (var item in fileList)
                {
                    //var file = _mapper.Map<FileTrackingRequestDto>(item);
                    var fileTracking = await _filesTrackingService.UploadSingleFileAsync(userId, _unitOfWork, item, "REC-PHOTO", "REC-PHOTO");

                    var receivingFile = new ReceivingFile
                    {
                        created_by = userId,
                        receiving_header_id = receivingHeaderId,
                        receiving_detail_id = receivingDetailId,
                        file_id = fileTracking.id
                    };

                    await _receivingFileRepository.CreateAsync(receivingFile);
                    await _unitOfWork.SaveChangesAsync();
                }
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task<List<string>> DeleteFilesAsync(string userId, DeleteReceivingFileDto request)
        {

            var rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == request.receiving_header_id && x.status_flag != RcvHeader.StatusFlag.Cancelled);

            if (rcvHeader == null)
                throw new NotFoundException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            var rcvDetail = await _receivingDetailRepository.GetByIdAsync(x => x.id == request.receiving_detail_id && x.status_flag != RcvDetail.StatusFlag.Cancelled);

            if (rcvDetail == null)
                throw new NotFoundException(ErrorCodes.ReceivingDetailNotFound, ErrorMessages.ReceivingDetailNotFound);

            var deletedFilePath = new List<string>();
            try
            {
                _unitOfWork.BeginTransaction();
                DateTime currentTimeStamp = DateTime.Now;
                if (request.files != null && request.files.Any())
                {
                    var receivingFiles = await _receivingFileRepository.GetDictionaryAsync
                        (x => x.receiving_header_id == request.receiving_header_id
                        && x.receiving_detail_id == request.receiving_detail_id
                        && request.files.Contains(x.id));

                    if (receivingFiles == null || receivingFiles.Count != request.files.Count)
                        throw new BadRequestException(ErrorCodes.ReceivingFileNotound, ErrorMessages.ReceivingFileNotound);

                    List<int> fileIds = receivingFiles.Select(x => x.Value.file_id).ToList();
                    deletedFilePath = await _filesTrackingService.RemoveFileAsync(_unitOfWork, fileIds);
                    await _receivingFileRepository.DeleteRangeAsync(receivingFiles.Values);
                    await _unitOfWork.SaveChangesAsync();
                }
                _unitOfWork.Commit();
                return deletedFilePath;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }


        public async Task<PagedResponse<PagedReceivingResponseDto>> PagedFilterReceivingAsync(SearchPagedRequestDto requests, PagedFilterReceivingRequestDto filters)
        {
            if (filters.received_date_to != null && filters.received_date_from != null && filters.received_date_from > filters.received_date_to)
            {
                return new PagedResponse<PagedReceivingResponseDto>();
            }
            if (filters.inspection_due_date_to != null && filters.inspection_due_date_from != null && filters.inspection_due_date_from > filters.inspection_due_date_to)
            {
                return new PagedResponse<PagedReceivingResponseDto>();
            }
            var pageable = PagingUtilities.GetPageable(requests.Page, requests.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            List<Expression<Func<ReceivingInfoModel, bool>>> cond = new List<Expression<Func<ReceivingInfoModel, bool>>>();
            if (!string.IsNullOrEmpty(requests.Keyword))
            {
                cond.Add(x => x.receiving_no.Contains(requests.Keyword)
                // || (x.pos != null && x.pos.Any(po => po.po_no.Contains(requests.Keyword)))
                || (x.container_no != null && x.container_no.Contains(requests.Keyword))
                || (x.asn_no != null && x.asn_no.Contains(requests.Keyword)));
            }
            if (filters.received_date_from != null)
            {
                cond.Add(x => x.received_on != null && x.received_on >= filters.received_date_from);
            }
            if (filters.received_date_to != null)
            {
                cond.Add(x => x.received_on != null && x.received_on <= filters.received_date_to);
            }
            if (filters.status != null && filters.status.Count > 0)
            {
                cond.Add(x => filters.status.Contains(x.status_flag));
            }
            if (filters.branch_plant_id != null && filters.branch_plant_id.Count > 0)
            {
                cond.Add(x => filters.branch_plant_id.Contains((int)x.branch_plant_id));
            }
            if (filters.supplier_id != null && filters.supplier_id.Count > 0)
            {
                cond.Add(x => x.suppliers != null && filters.supplier_id.Contains((int)x.suppliers.supplier_id));
            }
            if (!string.IsNullOrEmpty(filters.inspected_by))
            {
                cond.Add(x => x.inspected_by != null && filters.inspected_by.Contains(x.inspected_by));
            }
            if (filters.inspection_due_date_from != null)
            {
                cond.Add(x => x.inspection_end_on != null && x.inspection_end_on >= filters.inspection_due_date_from);
            }
            if (filters.inspection_due_date_to != null)
            {
                cond.Add(x => x.inspection_end_on != null && x.inspection_end_on <= filters.inspection_due_date_to);
            }

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = requests.SortBy ?? DefaultSortField.ReceivingHeader,
                    IsAscending = requests.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<ReceivingInfoModel>(listSort);
            PagedResponseModel<ReceivingInfoModel> result = await _receivingHeaderRepository.SearchReceivingAsync(cond, requests.Keyword, pageable, skipRow, listSort);

            return new PagedResponse<PagedReceivingResponseDto>
            {
                Items = _mapper.Map<List<PagedReceivingResponseDto>>(result.Items),
                TotalItems = result.TotalItems,
                TotalPage = (int)Math.Ceiling(result.TotalItems / (double)pageable.Size),
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }
        public async Task<List<DropdownListResponseDto>> GetReceivingListAsync(GetReceivingListRequestDto request)
        {
            Expression<Func<ReceivingHeader, bool>>? cond = null;

            if (request.status_list != null && request.status_list.Count > 0)
            {
                List<string> list1 = request.status_list.ToList();
                cond = x => list1.Contains(x.status_flag);
            }

            List<ReceivingHeader> result = await _receivingHeaderRepository.GetReceivingListAsync(cond);

            List<DropdownListResponseDto> list = result
                    .Select(x => new DropdownListResponseDto
                    {
                        id = x.id,
                        label = x.receiving_no
                    })
                    .ToList();

            return list;
        }
        public async Task<DocumentListResponseDto> GetDocumentList(int rcvHeaderId)
        {
            List<string> result = await _receivingDetailRepository.GetDocumentListAsync(rcvHeaderId);

            DocumentListResponseDto list = new DocumentListResponseDto();
            result.CopyTo(list.supplier_document_nos);
            return list;
        }
        public async Task<ReceivingGetByIdFileUrlResponseDTO> GetByReceivingNoAsync(string receivingNo)
        {
            Expression<Func<ReceivingInfoModel, bool>> cond = x => x.receiving_no == receivingNo;
            var result = await GetReceving(cond);
            return result;
        }
        public async Task<ReceivingGetByIdFileUrlResponseDTO> GetByReceivingHeaderIdAsync(int rcvHeaderId)
        {
            Expression<Func<ReceivingInfoModel, bool>> cond = x => x.receiving_header_id == rcvHeaderId;
            var result = await GetReceving(cond);
            return result;
        }
        public async Task<ReceivingItemListResponseDto> GetItemListAsync(int rcvHeaderId, string? documentNo, string? packageNo)
        {
            List<Expression<Func<ReceivingItemModel, bool>>> cond = new List<Expression<Func<ReceivingItemModel, bool>>>();
            cond.Add(x => x.receiving_header_id == rcvHeaderId);
            cond.Add(x => x.document_no == documentNo);
            cond.Add(x => x.package_no == packageNo);

            List<ReceivingItemModel> result = await _receivingDetailRepository.GetItemListAsync(cond, rcvHeaderId, documentNo, packageNo);

            ReceivingItemListResponseDto itemList = new ReceivingItemListResponseDto();
            itemList.items = _mapper.Map<List<ReceivingItemDTO>>(result);

            return itemList;
        }
        public async Task<ReceivingLineNoResponseDto> GetReceivingDetailLineByItemAsync(int rcvHeaderId, int itemId)
        {
            List<Expression<Func<ReceivingDetail, bool>>> cond = new List<Expression<Func<ReceivingDetail, bool>>>();
            cond.Add(x => x.receiving_header_id == rcvHeaderId);
            cond.Add(x => x.item_id == itemId);

            List<ReceivingDetail> result = await _receivingDetailRepository.GetReceivingDetailLineByItemAsync(cond);

            ReceivingLineNoResponseDto itemList = new ReceivingLineNoResponseDto();
            itemList.receiving_line_nos = result.Select(x => new ReceivingLineDetailDTO
            {
                receiving_detail_id = x.id,
                receiving_detail_line_no = x.line_no
            })
                    .ToList();

            return itemList;
        }
        public async Task<ReceivingItemDetailResponseDto> GetItemDetailsAsync(int rcvDetailId)
        {
            ReceivingItemDetailUomConversionModel itemDetail = await _receivingDetailRepository.GetItemDetailsAsync(rcvDetailId);

            if (itemDetail == null)
                throw new NotFoundException(ErrorMessages.ReceivingDetailNotFound);

            ReceivingItemDetailResponseDto item = new ReceivingItemDetailResponseDto()
            {
                primary_remaining_qty = itemDetail.remaining_qty,
                primary_total_qty = itemDetail.qty,
                primary_uom = itemDetail.uom,
                secondary_remaining_qty = itemDetail.uom == itemDetail.po_uom ? null : itemDetail.remaining_qty / itemDetail.primary_uom_qty * itemDetail.secondary_uom_qty,
                secondary_total_qty = itemDetail.uom == itemDetail.po_uom ? null : itemDetail.qty / itemDetail.primary_uom_qty * itemDetail.secondary_uom_qty,
                secondary_uom = itemDetail.po_uom,
                notes_to_warehouse = itemDetail.notes_to_warehouse,
                country_of_origin_id = itemDetail.country_of_origin_id,
                lot_no = itemDetail.lot_control_flag ? "LOT" + DateTime.Now.ToString("ddMMyyyy") : null,
                packing_list_info = _mapper.Map<packingListInfoDto>(itemDetail.packing_list_info)
            };

            return item;
        }
        public async Task<ReceivingDetailFileUrlResponseDto> SearchDetailAsync(int rcvHeaderId, string keyword, GetReceivingListRequestDto request)
        {
            ReceivingHeader rcvHeader = await _receivingHeaderRepository.GetByIdAsync(x => x.id == rcvHeaderId);
            if (rcvHeader == null)
                throw new NotFoundException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            List<Expression<Func<ReceivingItemDetailModel, bool>>> cond = new List<Expression<Func<ReceivingItemDetailModel, bool>>>();

            cond.Add(x => x.receiving_header_id == rcvHeaderId);

            if (request.status_list != null && request.status_list.Count > 0)
                cond.Add(x => request.status_list.Contains(x.status_flag));

            if (!string.IsNullOrEmpty(keyword))
                cond.Add(x => x.po_no.Contains(keyword) || x.item_no.Contains(keyword) || x.description_1.Contains(keyword)
                || (x.supplier_part_no != null && x.supplier_part_no.Contains(keyword))
                || (x.supplier_document_no != null && x.supplier_document_no.Contains(keyword)));

            List<ReceivingItemDetailModel> rcvDetailList = await _receivingDetailRepository.GetReceivingDetailsAsync(cond);
            List<ReceivingFileModel> rcvDetailFileList = await _receivingFileRepository.GetFileInfoAsync(rcvHeaderId);

            ReceivingDetailFileUrlResponseDto rcv = new ReceivingDetailFileUrlResponseDto();
            rcv.receiving_header_id = rcvHeader.id;
            rcv.receiving_no = rcvHeader.receiving_no;

            rcv.receiving_details = _mapper.Map<List<ReceivingDetailBaseResponseDto>>(rcvDetailList);
            rcv.photos = _mapper.Map<List<RcvFileDetail>>(rcvDetailFileList);

            return rcv;
        }



        public async Task<int> CreateReceivingHeader(string userId, IUnitOfWork unitOfWork, ReceivingHeader request)
        {
            // - Receiving No is formatted as RECYYMM99999
            var seq = await _sequenceTrackingRepository.GetSequenceNoByType(SequenceTracking.Type.Receiving);

            request.receiving_no = $"{SequenceTracking.Type.Receiving}{DateTime.Now:yyMM}{seq:00000}";
            request.created_by = userId;
            request.created_on = DateTime.Now;

            await _receivingHeaderRepository.CreateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return request.id;
        }
        public async Task CreateReceivingDetail(string userId, IUnitOfWork unitOfWork, List<ReceivingDetail> requests)
        {
            //get all item detail
            var itemIds = requests.Select(x => x.item_id).Distinct().ToList();
            List<Item> itemList = await _itemRepository.GetItemListByIdsAsync(itemIds);
            List<ItemUomConversion> itemUomConversionList = await _itemUomConversionRepository.GetItemUomConversionListAsync(itemIds);
            DateTime currentTimestamp = DateTime.Now;
            List<ReceivingDetail> detailList = new List<ReceivingDetail>();
            foreach (ReceivingDetail detail in requests)
            {
                Item itemDetail = itemList.FirstOrDefault(x => x.id == detail.item_id);
                if (detail.po_uom != itemDetail.primary_uom)
                {
                    ItemUomConversion itemUomConversionDetail = itemUomConversionList.FirstOrDefault(x => x.item_id == detail.item_id);
                    if (itemUomConversionDetail == null)
                        throw new BadRequestException(ErrorCodes.ItemUomConversionNotFound, ErrorMessages.ItemUomConversionNotFound);
                    //Update ReceivingDetail.qty to(po_qty / ItemUomConversion.secondary_uom_qty) *ItemUomConversion.primary_uom_qty)
                    detail.qty = (detail.qty / itemUomConversionDetail.secondary_uom_qty) / itemUomConversionDetail.primary_uom_qty;
                    // detail.uom = itemDetail.primary_uom;
                }
                detail.uom = itemDetail.primary_uom;
                detail.created_by = userId;
                detail.created_on = currentTimestamp;
                detailList.Add(detail);
            }
            await _receivingDetailRepository.CreateRangeAsync(detailList);
            await _unitOfWork.SaveChangesAsync();
        }


        #region Private Methods
        private async Task validatePoDetailForReceiving(int poDetailId, int qtyToReceive)
        {
            var poDetail = await _poDetailRepository.GetByIdAsync(x => x.id == poDetailId);
            if (poDetail == null)
                throw new BadRequestException(ErrorCodes.PODetailNotFound, ErrorMessages.PODetailNotFound);
            if (poDetail.status_flag == PurchaseOrderDetail.StatusFlag.Cancelled)
                throw new BadRequestException(ErrorCodes.PODetailCancelled, ErrorMessages.PODetailCancelled);
            if (poDetail.open_qty < qtyToReceive)
                throw new BadRequestException(ErrorCodes.ReceivedQtyMoreThanOpenQty, ErrorMessages.ReceivedQtyMoreThanOpenQty);
        }
        private async Task validateASNDetailForReceiving(int asnHeaderId, int poDetailId, int qtyToReceive)
        {
            var asnDetail = await _asnDetailRepository.GetByIdAsync(x => x.asn_header_id == asnHeaderId && x.id == poDetailId);
            if (asnDetail == null)
                throw new BadRequestException(ErrorCodes.ASNDetailNotFound, ErrorMessages.ASNDetailNotFound);
            if (asnDetail.status_flag == AdvancedShipmentNoticesDetail.StatusFlag.Cancelled)
                throw new BadRequestException(ErrorCodes.ASNDetailCancelled, ErrorMessages.ASNDetailCancelled);
            if (asnDetail.qty < qtyToReceive)
            {
                //check po detail
                var poDetail = await _poDetailRepository.GetByIdAsync(x => x.id == poDetailId);
                if (poDetail.open_qty < asnDetail.qty - qtyToReceive)
                    throw new BadRequestException(ErrorCodes.ReceivedQtyMoreThanOpenQty, ErrorMessages.ReceivedQtyMoreThanOpenQty);
            }
        }
        private async Task<ReceivingGetByIdFileUrlResponseDTO> GetReceving(Expression<Func<ReceivingInfoModel, bool>> filter)
        {
            List<Expression<Func<ReceivingInfoModel, bool>>> cond = new List<Expression<Func<ReceivingInfoModel, bool>>>();
            cond.Add(filter);
            SearchPagedRequestDto requests = new SearchPagedRequestDto();
            var pageable = PagingUtilities.GetPageable(requests.Page, requests.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = requests.SortBy ?? DefaultSortField.ReceivingHeader,
                    IsAscending = requests.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<ReceivingInfoModel>(listSort);
            PagedResponseModel<ReceivingInfoModel> result = await _receivingHeaderRepository.SearchReceivingAsync(cond, null, pageable, skipRow, listSort);
            if (result.Items == null || result.Items.Count() == 0)
                throw new NotFoundException(ErrorCodes.ReceivingHeaderNotFound, ErrorMessages.ReceivingHeaderNotFound);

            ReceivingInfoModel receiving = result.Items.ElementAt(0);
            var rcv = _mapper.Map<ReceivingGetByIdFileUrlResponseDTO>(receiving);
            List<Expression<Func<ReceivingItemDetailModel, bool>>> detailCond = new List<Expression<Func<ReceivingItemDetailModel, bool>>>();

            detailCond.Add(x => x.receiving_header_id == receiving.receiving_header_id);

            List<ReceivingItemDetailModel> rcvDetailList = await _receivingDetailRepository.GetReceivingDetailsAsync(detailCond);
            List<ReceivingFileModel> rcvDetailFileList = await _receivingFileRepository.GetFileInfoAsync(receiving.receiving_header_id);

            rcv.receiving_details = _mapper.Map<List<ReceivingDetailBaseResponseDto>>(rcvDetailList);
            rcv.photos = _mapper.Map<List<RcvFileDetail>>(rcvDetailFileList);
            return rcv;
        }
        #endregion
    }
}