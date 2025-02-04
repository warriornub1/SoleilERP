using AutoMapper;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Application.Transactions.AdvancedShipmentNotices.Interfaces;
using SERP.Application.Transactions.FilesTracking.Interfaces;
using SERP.Application.Transactions.Invoices.DTOs.Request;
using SERP.Application.Transactions.Invoices.DTOs.Request.Base;
using SERP.Application.Transactions.Invoices.DTOs.Response;
using SERP.Application.Transactions.Invoices.Interfaces;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Common.Model;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Transactions.AdvancedShipmentNotices;
using SERP.Domain.Transactions.FilesTracking;
using SERP.Domain.Transactions.Invoice;
using SERP.Domain.Transactions.Invoice.Model;
using SERP.Domain.Transactions.PurchaseOrders;
using static SERP.Domain.Common.Constants.DomainConstant;
using static SERP.Domain.Common.Enums.SERPEnum;
using InvoiceDetail = SERP.Domain.Transactions.Invoice.InvoiceDetail;
using InvoiceHeader = SERP.Domain.Transactions.Invoice.InvoiceHeader;

namespace SERP.Application.Transactions.Invoices.Service
{
    internal class InvoiceService : IInvoiceService
    {
        private readonly IASNDetailRepository _asnDetailRepository;
        private readonly IInvoiceHeaderRepository _invoiceHeaderRepository;
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly IFileTrackingRepository _fileTrackingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IPODetailRepository _poDetailRepository;
        private readonly IInvoiceFileRepository _invoiceFileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierRepository _supplierRepository;

        public InvoiceService(
            IASNDetailRepository asnDetailRepository,
            IInvoiceHeaderRepository invoiceHeaderRepository,
            IInvoiceDetailRepository invoiceDetailRepository,
            IFileTrackingRepository fileTrackingRepository,
            IMapper mapper,
            ILogger<InvoiceService> logger,
            IPODetailRepository poDetailRepository,
            IInvoiceFileRepository invoiceFileRepository,
            IUnitOfWork unitOfWork,
            ISupplierRepository supplierRepository)
        {
            _asnDetailRepository = asnDetailRepository;
            _invoiceHeaderRepository = invoiceHeaderRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _fileTrackingRepository = fileTrackingRepository;
            _mapper = mapper;
            _logger = logger;
            _poDetailRepository = poDetailRepository;
            _invoiceFileRepository = invoiceFileRepository;
            _unitOfWork = unitOfWork;
            _supplierRepository = supplierRepository;
        }

        public PagedResponse<InvoicePagedResponseDto> SearchPaged(SearchPagedRequestDto request, FilterInvoicePagedRequestDto filter)
        {
            var query = _invoiceHeaderRepository.BuildInvoicePagedQuery(new FilterInvoicePagedRequestModel()
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                branch_plant_id = filter.branch_plant_id,
                supplier_id = filter.supplier_id,
                status = filter.status
            }, out var totalRows);

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.InvoiceHeader,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<InvoicePagedResponseDetail>(listSort);

            if (totalRows == 0)
            {
                return new PagedResponse<InvoicePagedResponseDto>();
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            var result = _mapper.Map<List<InvoicePagedResponseDto>>(pagedResponse);

            return new PagedResponse<InvoicePagedResponseDto>
            {
                Items = result,
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }

        public PagedResponse<InvoiceDetailPagedResponseDto> SearchDetail(SearchPagedRequestDto request, FilterInvoiceDetailRequestDto filter)
        {
            var query = _invoiceDetailRepository.BuildInvoiceDetailPagedQuery(new FilterInvoiceDetailPagedRequestModel()
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                invoice_id = filter.invoice_id,
                asn_header_id = filter.asn_header_id,
                po_header_id = filter.po_header_id,
                status = filter.status
            }, out var totalRows);

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.InvoiceDetail,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<InvoiceDetailPagedResponseDetail>(listSort);

            if (totalRows == 0)
            {
                return new PagedResponse<InvoiceDetailPagedResponseDto>();
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            var result = _mapper.Map<List<InvoiceDetailPagedResponseDto>>(pagedResponse);

            return new PagedResponse<InvoiceDetailPagedResponseDto>
            {
                Items = result,
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            };
        }

        public async Task<InvoiceInfoResponseDto> GetByIdAsync(int id)
        {
            var result = await _invoiceHeaderRepository.GetInvoiceAsync(x => x.id == id);
            if (result == null)
            {
                throw new NotFoundException(string.Format(ErrorMessages.InvoiceHeaderNotFound, nameof(InvoiceHeader.id), id));
            }

            return _mapper.Map<InvoiceInfoResponseDto>(result);
        }

        public async Task<InvoiceInfoResponseDto> GetByInvoiceNoAsync(string invoiceNo)
        {
            var result = await _invoiceHeaderRepository.GetInvoiceAsync(x => x.invoice_no == invoiceNo);
            if (result == null)
            {
                throw new NotFoundException(string.Format(ErrorMessages.InvoiceHeaderNotFound, nameof(InvoiceHeader.invoice_no), invoiceNo));
            }

            return _mapper.Map<InvoiceInfoResponseDto>(result);
        }

        public async Task<int[]> CreateInvoiceAsync(string userId, CreateInvoiceRequestDto request)
        {
            await ValidateRequestAsync(request.invoices.Select(x => new ValidateInvoiceRequest
            {
                invoiceHeader = _mapper.Map<InvoiceHeaderRequestDto>(x.invoice_header),
                invoiceDetails = _mapper.Map<List<InvoiceDetailRequestDto>>(x.invoice_details)
            }).ToList());

            var invoiceHeaderToInsert = new List<InvoiceHeader>();
            var invoiceDetailToInsert = new List<InvoiceDetail>();
            var invoiceMappingList = new List<InvoiceMapping>();
            var currentDateTime = DateTime.Now;
            var lineNo = 0;
            foreach (var invoiceRequestDto in request.invoices)
            {
                var invoiceMappingItem = new InvoiceMapping();

                // Invoice Header
                if (invoiceRequestDto.invoice_header != null)
                {
                    var invoiceMapped = new InvoiceHeader
                    {
                        invoice_no = invoiceRequestDto.invoice_header.invoice_no,
                        asn_header_id = null,
                        receiving_header_id = null,
                        branch_plant_id = invoiceRequestDto.invoice_header.branch_plant_id,
                        supplier_id = invoiceRequestDto.invoice_header.supplier_id,
                        status_flag = request.action.Equals(DomainConstant.Action.Submit)
                            ? DomainConstant.InvoiceHeader.StatusFlag.New
                            : DomainConstant.InvoiceHeader.StatusFlag.Draft,
                        currency_id = invoiceRequestDto.invoice_header.currency_id,
                        amt = invoiceRequestDto.invoice_header.amt,
                        total_amt = invoiceRequestDto.invoice_header.total_amt,
                        variance_reason = invoiceRequestDto.invoice_header.variance_reason,
                        invoice_date = invoiceRequestDto.invoice_header.invoice_date,
                        created_on = currentDateTime,
                        created_by = userId
                    };

                    invoiceHeaderToInsert.Add(invoiceMapped);
                    invoiceMappingItem.InvoiceHeader = invoiceMapped;
                }

                // Invoice Detail
                if (invoiceRequestDto.invoice_details != null)
                {
                    var invoiceDetailMapped = new List<InvoiceDetail>();
                    foreach (var invoiceDetailRequestDto in invoiceRequestDto.invoice_details)
                    {
                        // invoice_details.total_price is unit_price * qty
                        var totalPrice = invoiceDetailRequestDto.unit_price * invoiceDetailRequestDto.qty;

                        var invoiceDetail = new InvoiceDetail
                        {
                            invoice_header_id = 0,
                            line_no = ++lineNo,
                            po_detail_id = invoiceDetailRequestDto.po_detail_id,
                            qty = invoiceDetailRequestDto.qty,
                            unit_price = invoiceDetailRequestDto.unit_price,
                            exchange_rate = invoiceDetailRequestDto.exchange_rate,
                            total_price = totalPrice,
                            country_of_origin_id = invoiceDetailRequestDto.country_of_origin_id,
                            created_on = currentDateTime,
                            created_by = userId
                        };

                        invoiceDetailMapped.Add(invoiceDetail);
                    }

                    invoiceMappingItem.InvoiceDetails = invoiceDetailMapped;
                }

                invoiceMappingList.Add(invoiceMappingItem);
            }

            try
            {
                _unitOfWork.BeginTransaction();

                if (invoiceHeaderToInsert.Count > 0)
                {
                    await _invoiceHeaderRepository.CreateRangeAsync(invoiceHeaderToInsert);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Created {invoiceHeaderToInsert.Count} invoice headers.");
                }

                if (invoiceMappingList.Count > 0)
                {
                    foreach (var invoiceMapping in invoiceMappingList)
                    {
                        foreach (var invoiceDetail in invoiceMapping.InvoiceDetails)
                        {
                            // Set invoice_header_id to invoice_detail
                            invoiceDetail.invoice_header_id = invoiceMapping.InvoiceHeader.id;
                            invoiceDetailToInsert.Add(invoiceDetail);
                        }
                    }
                }

                if (invoiceDetailToInsert.Count > 0)
                {
                    await _invoiceDetailRepository.CreateRangeAsync(invoiceDetailToInsert);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Created {invoiceDetailToInsert.Count} invoice details.");
                }

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                throw;
            }

            return invoiceHeaderToInsert.Select(x => x.id).ToArray();
        }

        public async Task UpdateInvoiceAsync(string userId, UpdateInvoiceRequestDto request)
        {
            await ValidateRequestAsync(request.invoices.Select(x => new ValidateInvoiceRequest
            {
                invoiceHeader = _mapper.Map<InvoiceHeaderRequestDto>(x.invoice_header),
                invoiceDetails = _mapper.Map<List<InvoiceDetailRequestDto>>(x.invoice_details)
            }).ToList());

            var invoiceHeaderToUpdate = new List<InvoiceHeader>();
            var invoiceDetailsToInsert = new List<InvoiceDetail>();
            var invoiceDetailsToUpdate = new List<InvoiceDetail>();
            var invoiceMappingList = new List<InvoiceMapping>();
            var currentDateTime = DateTime.Now;

            var invoiceHeaderIds = request.invoices.Where(x => x.invoice_header != null)
                .Select(x => x.invoice_header!.id).ToArray();
            var dictInvoiceHeader = new Dictionary<int, InvoiceHeader>();
            if (invoiceHeaderIds.Length > 0)
            {
                dictInvoiceHeader = await _invoiceHeaderRepository.GetDictionaryAsync(x => invoiceHeaderIds.Contains(x.id));
            }

            var dictInvoiceDetail = new Dictionary<int, InvoiceDetail>();
            var poDetailIds = new List<int>();
            var invoiceDetailFromRequestDto = request.invoices.Where(x => x.invoice_details != null)
                .SelectMany(x => x.invoice_details!).ToList();
            var invoiceDetailIds = new List<int>();
            if (invoiceDetailFromRequestDto.Count > 0)
            { 
                invoiceDetailIds = invoiceDetailFromRequestDto.Select(y => y.id).ToList();

                //poDetailIds = invoiceDetailRequestDto.Select(x => x.po_detail_id).Distinct().ToList();
            }

            if (request.delete_detail_id is not null && request.delete_detail_id.Count > 0)
            {
                invoiceDetailIds.AddRange(request.delete_detail_id);
            }

            if (invoiceDetailIds.Count > 0)
            {
                dictInvoiceDetail = await _invoiceDetailRepository.GetDictionaryAsync(x => invoiceDetailIds.Contains(x.id));
            }

            //var poDetailList = await GetPoDetailList(poDetailIds);

            var invoiceDetailToDelete = new List<InvoiceDetail>();
            if (request.delete_detail_id is not null && request.delete_detail_id.Count > 0)
            {
                foreach (var detailId in request.delete_detail_id)
                {
                    if (!dictInvoiceDetail.TryGetValue(detailId, out var invoiceDetail))
                    {
                        throw new NotFoundException(string.Format(ErrorMessages.InvoiceDetailNotFound, nameof(detailId), detailId));
                    }

                    invoiceDetailToDelete.Add(invoiceDetail);
                }
            }

            foreach (var invoiceRequestDto in request.invoices)
            {
                // update invoice_header
                if (invoiceRequestDto.invoice_header is not null)
                {
                    if (!dictInvoiceHeader.TryGetValue(invoiceRequestDto.invoice_header.id, out var invoiceHeader))
                    {
                        throw new NotFoundException(string.Format(ErrorMessages.InvoiceHeaderNotFound, nameof(invoiceRequestDto.invoice_header.id), invoiceRequestDto.invoice_header.id));
                    }

                    invoiceHeader.invoice_no = invoiceRequestDto.invoice_header.invoice_no;
                    invoiceHeader.asn_header_id = null;
                    invoiceHeader.receiving_header_id = null;
                    invoiceHeader.branch_plant_id = invoiceRequestDto.invoice_header.branch_plant_id;
                    invoiceHeader.supplier_id = invoiceRequestDto.invoice_header.supplier_id;
                    invoiceHeader.status_flag = request.action.Equals(DomainConstant.Action.Submit)
                        ? DomainConstant.InvoiceHeader.StatusFlag.New
                        : DomainConstant.InvoiceHeader.StatusFlag.Draft;
                    invoiceHeader.currency_id = invoiceRequestDto.invoice_header.currency_id;
                    invoiceHeader.amt = invoiceRequestDto.invoice_header.amt;
                    invoiceHeader.total_amt = invoiceRequestDto.invoice_header.total_amt;
                    invoiceHeader.variance_reason = invoiceRequestDto.invoice_header.variance_reason;
                    invoiceHeader.invoice_date = invoiceRequestDto.invoice_header.invoice_date;
                    invoiceHeader.last_modified_by = userId;
                    invoiceHeader.last_modified_on = currentDateTime;
                    invoiceHeaderToUpdate.Add(invoiceHeader);
                }

                // update invoice_details
                if (invoiceRequestDto.invoice_details is not null && invoiceRequestDto.invoice_details.Count > 0)
                {
                    var lineNo = 0;
                    if (invoiceRequestDto.invoice_header is not null && dictInvoiceDetail.Values.Count > 0)
                    {
                        lineNo = dictInvoiceDetail.Values
                            .Where(x => x.invoice_header_id == invoiceRequestDto.invoice_header.id)
                            .Max(x => x.line_no);
                    }

                    foreach (var invoiceDetailRequestDto in invoiceRequestDto.invoice_details)
                    {
                        var mode = Mode.Update;
                        dictInvoiceDetail.TryGetValue(invoiceDetailRequestDto.id, out var invoiceDetail);
                        if (invoiceDetail is null)
                        {
                            mode = Mode.Insert;
                            invoiceDetail = new InvoiceDetail();
                        }

                        var totalPrice = invoiceDetailRequestDto.unit_price * invoiceDetailRequestDto.qty;
                        invoiceDetail.line_no = ++lineNo;
                        invoiceDetail.po_detail_id = invoiceDetailRequestDto.po_detail_id;
                        invoiceDetail.qty = invoiceDetailRequestDto.qty;
                        invoiceDetail.exchange_rate = invoiceDetailRequestDto.exchange_rate;
                        invoiceDetail.unit_price = invoiceDetailRequestDto.unit_price;
                        invoiceDetail.total_price = totalPrice;
                        invoiceDetail.country_of_origin_id = invoiceDetailRequestDto.country_of_origin_id;

                        switch (mode)
                        {
                            case Mode.Insert:
                                {
                                    invoiceDetail.invoice_header_id = invoiceRequestDto.invoice_header?.id ?? 0;
                                    invoiceDetail.created_by = userId;
                                    invoiceDetail.created_on = currentDateTime;
                                    invoiceDetailsToInsert.Add(invoiceDetail);
                                    break;
                                }
                            case Mode.Update:
                                {
                                    invoiceDetail.last_modified_by = userId;
                                    invoiceDetail.last_modified_on = currentDateTime;
                                    invoiceDetailsToUpdate.Add(invoiceDetail);
                                    break;
                                }
                        }
                    }
                }

                try
                {
                    _unitOfWork.BeginTransaction();

                    if (invoiceHeaderToUpdate.Count > 0)
                    {
                        await _invoiceHeaderRepository.UpdateRangeAsync(invoiceHeaderToUpdate);
                        _logger.LogInformation($"InvoiceHeader updated: {invoiceHeaderToUpdate.Count}");
                    }

                    if (invoiceDetailsToInsert.Count > 0)
                    {
                        await _invoiceDetailRepository.CreateRangeAsync(invoiceDetailsToInsert);
                        _logger.LogInformation($"InvoiceDetail inserted: {invoiceDetailsToInsert.Count}");
                    }

                    if (invoiceDetailsToUpdate.Count > 0)
                    {
                        await _invoiceDetailRepository.UpdateRangeAsync(invoiceDetailsToUpdate);
                        _logger.LogInformation($"InvoiceDetail updated: {invoiceDetailsToUpdate.Count}");
                    }

                    if (invoiceDetailToDelete.Count > 0)
                    {
                        await _invoiceDetailRepository.DeleteRangeAsync(invoiceDetailToDelete);
                        _logger.LogInformation($"InvoiceDetail deleted: {invoiceDetailToDelete.Count}");
                    }

                    await _unitOfWork.SaveChangesAsync();
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    _logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }

        public async Task DeleteInvoiceAsync(string userId, int invoiceHeaderId)
        {
            var invoiceHeader = await _invoiceHeaderRepository.GetByIdAsync(x => x.id == invoiceHeaderId);
            if (invoiceHeader is null)
            {
                throw new NotFoundException(ErrorCodes.PurchaseOrderNotFound,
                    string.Format(ErrorMessages.InvoiceHeaderNotFound, nameof(InvoiceHeader.id), invoiceHeaderId));
            }

            var invoiceDetails = await GetInvoiceDetailAsync([invoiceHeaderId]);

            switch (invoiceHeader.status_flag)
            {
                // - Update Invoice Header and Detail to status 90: Cancelled if status is 02: New
                case PurchaseOrder.StatusFlag.New:
                    {
                        ////-If Invoice Header Status is 02: New.All Details status should be 02:New or 90:Cancelled
                        //if (invoiceDetails.Any(x => x.status_flag != DomainConstant.InvoiceHeader.StatusFlag.New &&
                        //                            x.status_flag != DomainConstant.InvoiceHeader.StatusFlag.Cancelled))
                        //{
                        //    throw new BadRequestException(ErrorCodes.ValidationError,
                        //        ErrorMessages.CannotDeletePOStatusNew);
                        //}

                        invoiceHeader.status_flag = DomainConstant.InvoiceHeader.StatusFlag.Cancelled;
                        invoiceHeader.last_modified_by = userId;
                        invoiceHeader.last_modified_on = DateTime.Now;

                        //foreach (var invoiceDetail in invoiceDetails)
                        //{
                        //    invoiceDetail.status_flag = DomainConstant.InvoiceHeader.StatusFlag.Cancelled;
                        //    invoiceDetail.last_modified_by = userId;
                        //    invoiceDetail.last_modified_on = DateTime.Now;
                        //}

                        try
                        {
                            _unitOfWork.BeginTransaction();
                            await _invoiceHeaderRepository.UpdateAsync(invoiceHeader);
                            //await _invoiceDetailRepository.UpdateRangeAsync(invoiceDetails);
                            await _unitOfWork.SaveChangesAsync();
                            _unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            _unitOfWork.Rollback();
                            _logger.LogError(ex, $"Delete PO Error: {ex.Message}");
                            throw;
                        }

                        break;
                    }
                // - Delete Invoice Header and Detail if status is 01: Draft
                case PurchaseOrder.StatusFlag.Draft:
                    {
                        ////-If Invoice Header Status is 01: Draft.All Invoice Details status should be 01: Draft
                        //if (invoiceDetails.Any(x => x.status_flag != PurchaseOrder.StatusFlag.Draft))
                        //{
                        //    throw new BadRequestException(ErrorCodes.ValidationError,
                        //        ErrorMessages.CannotDeletePOStatusDraft);
                        //}

                        try
                        {
                            _unitOfWork.BeginTransaction();
                            await _invoiceHeaderRepository.DeleteAsync(invoiceHeader);
                            await _invoiceDetailRepository.DeleteRangeAsync(invoiceDetails);
                            await _unitOfWork.SaveChangesAsync();
                            _unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            _unitOfWork.Rollback();
                            _logger.LogError(ex, $"Delete PO Error: {ex.Message}");
                            throw;
                        }

                        break;
                    }
            }
        }

        public async Task<int[]> UploadFileAsync(string userId, UploadInvoiceFileRequestDto request)
        {
            var isExistedHeader = await _invoiceHeaderRepository.CheckInvoiceHeaderExistedAsync(request.invoice_header_id);

            if (!isExistedHeader)
            {
                throw new NotFoundException(string.Format(ErrorMessages.InvoiceHeaderNotFound, nameof(InvoiceHeader.id), request.invoice_header_id));
            }

            var fileTrackingToInsert = new List<FileTracking>();

            foreach (var item in request.files)
            {
                var fileTracking = new FileTracking
                {
                    created_by = userId,
                    file_type = item.file.ContentType,
                    file_name = item.file.FileName,
                    upload_source = request.upload_source,
                    document_type = request.document_type,
                    url_path = item.url_path,
                    file_size = Utilities.ConvertFileLengthToMegabytes(item.file.Length),
                };

                fileTrackingToInsert.Add(fileTracking);
            }

            if (fileTrackingToInsert.Count == 0)
            {
                return [];
            }

            int[] invoiceFileIDs;
            try
            {
                _unitOfWork.BeginTransaction();
                await _fileTrackingRepository.CreateRangeAsync(fileTrackingToInsert);
                await _unitOfWork.SaveChangesAsync();

                var poFileToInsert = fileTrackingToInsert.Select(x => new InvoiceFile()
                {
                    created_by = userId,
                    invoice_header_id = request.invoice_header_id,
                    file_id = x.id,
                }).ToList();

                if (poFileToInsert.Count > 0)
                {
                    await _invoiceFileRepository.CreateRangeAsync(poFileToInsert);
                }

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                invoiceFileIDs = poFileToInsert.Select(x => x.id).ToArray();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw;
            }

            return invoiceFileIDs;
        }

        public async Task<List<string>> RemoveFileAsync(int invoiceHeaderId, List<int> invoiceFileIDs)
        {
            var invoiceFiles = await GetInvoiceFileByInvoiceHeaderId(invoiceHeaderId, invoiceFileIDs);

            if (invoiceFiles is null)
            {
                throw new NotFoundException(string.Format(ErrorMessages.InvoiceFileNotFound, invoiceHeaderId));
            }

            var listFileTrackingIDs = invoiceFiles.Select(x => x.file_id).Distinct().ToHashSet();
            var fileTracking = await _fileTrackingRepository.GetFileTrackingByIds(listFileTrackingIDs);

            if (fileTracking is null)
            {
                throw new NotFoundException(ErrorCodes.FileTrackingNotFound,
                    string.Format(ErrorMessages.FileTrackingNotFound, invoiceFileIDs));
            }

            var filePath = fileTracking.Select(x => x.url_path).ToList();
            try
            {
                _unitOfWork.BeginTransaction();

                await _invoiceFileRepository.DeleteRangeAsync(invoiceFiles);
                await _fileTrackingRepository.DeleteRangeAsync(fileTracking);

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                _logger.LogError(e, e.Message);
                throw;
            }

            return filePath;
        }

        #region Private Methods
        private async Task<List<InvoiceFile>> GetInvoiceFileByInvoiceHeaderId(int invoiceHeaderId, List<int> invoiceFileIDs)
        {
            var invoiceFile = await _invoiceFileRepository.Find(x => x.invoice_header_id == invoiceHeaderId && invoiceFileIDs.Contains(x.id));
            return invoiceFile.ToList();
        }

        private async Task ValidateRequestAsync(List<ValidateInvoiceRequest> request)
        {
            // - supplier_id is valid in Supplier table with status flag of E
            var supplierIds = request.Where(x => x.invoiceHeader.supplier_id > 0)
                .Select(x => x.invoiceHeader.supplier_id).Distinct().ToHashSet();
            var supplierExisted = await _supplierRepository.GetSupplierAvailable(supplierIds);
            var invalidSupplierIds = supplierIds.Except(supplierExisted).ToArray();
            if (invalidSupplierIds.Length > 0)
            {
                throw new BadRequestException(string.Format(ErrorMessages.InvalidDataFromRequest,
                    nameof(POHeader.supplier_id), string.Join(", ", invalidSupplierIds),
                    nameof(Supplier), nameof(DomainConstant.StatusFlag.Enabled)));
            }

            // - po_detail_id is valid in PODetail with status of 02 or 11. 
            var poDetailIDs = request.SelectMany(x => x.invoiceDetails).Where(x => x.po_detail_id > 0)
                .Select(x => x.po_detail_id).Distinct().ToHashSet();

            if (poDetailIDs.Count > 0)
            {
                var poDetailExisted = await _poDetailRepository.GetPoDetailAvailable(poDetailIDs,
                [PurchaseOrder.StatusFlag.New, PurchaseOrder.StatusFlag.InProcess]);

                var invalidPoDetailIDs = poDetailIDs.Except(poDetailExisted).ToArray();
                if (invalidPoDetailIDs.Length > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.InvalidDataFromRequest,
                        nameof(InvoiceDetail.po_detail_id), string.Join(", ", invalidPoDetailIDs), nameof(PODetail),
                        $"{nameof(PurchaseOrder.StatusFlag.New)} or {nameof(PurchaseOrder.StatusFlag.InProcess)}"));
                }
            }

            // country_of_origin_id exists in Country table with status flag of E
        }

        private async Task<List<InvoiceHeader>> GetInvoiceHeaderAsync(List<int> invoiceHeaderId)
        {
            if (invoiceHeaderId.Count == 0)
            {
                return [];
            }

            var invoiceHeaders = await _invoiceHeaderRepository.Find(x => invoiceHeaderId.Contains(x.id));
            return invoiceHeaders.ToList();
        }

        private async Task<List<InvoiceDetail>> GetInvoiceDetailAsync(List<int> invoiceHeaderIds)
        {
            if (invoiceHeaderIds.Count == 0)
            {
                return [];
            }

            var invoiceDetails = await _invoiceDetailRepository.Find(x => invoiceHeaderIds.Contains(x.invoice_header_id));
            return invoiceDetails.ToList();
        }

        private async Task<List<PODetail>> GetPoDetailListAsync(List<int> poDetailIds)
        {
            if (poDetailIds.Count == 0)
            {
                return [];
            }

            var query = await _poDetailRepository.Find(x => poDetailIds.Contains(x.id));

            return query.ToList();
        }
        #endregion
    }
}
