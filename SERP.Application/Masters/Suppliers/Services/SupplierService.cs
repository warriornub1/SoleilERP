using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Dto;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Application.Masters.Currencies.Interfaces;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Application.Masters.Lovs.Interfaces;
using SERP.Application.Masters.Ports.Interfaces;
using SERP.Application.Masters.Sites.Interfaces;
using SERP.Application.Masters.Suppliers.DTOs;
using SERP.Application.Masters.Suppliers.DTOs.Request;
using SERP.Application.Masters.Suppliers.DTOs.Response;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Common.Model;
using SERP.Domain.Masters.Countries;
using SERP.Domain.Masters.Currencies;
using SERP.Domain.Masters.Items;
using SERP.Domain.Masters.LOVs;
using SERP.Domain.Masters.Ports;
using SERP.Domain.Masters.Sites;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Suppliers.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierSecondaryRepository _supplierSecondaryRepository;
        private readonly ISupplierItemMappingRepository _supplierItemMappingRepository;
        private readonly ISupplierSelfCollectSiteRepository _supplierSelfCollectSiteRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IPortRepository _portRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IIntermediarySupplierRepository _intermediarySupplierRepository;
        private readonly ILovRepository _lovRepository;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(
            ICountryRepository countryRepository,
            ICurrencyRepository currencyRepository,
            ISupplierRepository supplierRepository,
            ISupplierSecondaryRepository supplierSecondaryRepository,
            ISupplierItemMappingRepository supplierItemMappingRepository,
            ISupplierSelfCollectSiteRepository supplierSelfCollectSiteRepository,
            ISiteRepository siteRepository,
            IPortRepository portRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IItemRepository itemRepository,
            IIntermediarySupplierRepository intermediarySupplierRepository,
            ILovRepository lovRepository,
            ILogger<SupplierService> logger)
        {
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _supplierRepository = supplierRepository;
            _supplierSecondaryRepository = supplierSecondaryRepository;
            _supplierItemMappingRepository = supplierItemMappingRepository;
            _supplierSelfCollectSiteRepository = supplierSelfCollectSiteRepository;
            _siteRepository = siteRepository;
            _portRepository = portRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _intermediarySupplierRepository = intermediarySupplierRepository;
            _lovRepository = lovRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SupplierLimitedDto>> GetAllLimited(bool onlyEnabled)
        {
            return _mapper.Map<List<SupplierLimitedDto>>(await _supplierRepository.GetAllLimited(onlyEnabled));
        }

        public async Task<SupplierDto> GetById(int id)
        {
            return _mapper.Map<SupplierDto>(await _supplierRepository.GetById(id));
        }

        public async Task<List<SecondarySupplierLimitedDto>> GetSecondarySupplierLimited(int supplierId, bool onlyEnabled)
        {
            var secondarySuppliers = await _supplierRepository.GetSecondarySupplierLimited(supplierId, onlyEnabled);
            var secondarySupplierLimitedDtos = new List<SecondarySupplierLimitedDto>();

            foreach (var secondarySupplier in secondarySuppliers)
            {
                var secondarySupplierLimitedDto = new SecondarySupplierLimitedDto();
                secondarySupplierLimitedDto.secondary_supplier_id = secondarySupplier.id;
                secondarySupplierLimitedDto.secondary_supplier_no = secondarySupplier.sec_supplier_no;
                secondarySupplierLimitedDto.secondary_supplier_name = secondarySupplier.sec_supplier_name;
                secondarySupplierLimitedDto.secondary_supplier_status_flag = secondarySupplier.status_flag;
                secondarySupplierLimitedDtos.Add(secondarySupplierLimitedDto);
            }

            return secondarySupplierLimitedDtos;
        }

        public async Task<List<SupplierItemMappingDto>> GetSupplierItemMapping(int supplierId, int itemId, bool onlyEnabled)
        {
            return _mapper.Map<List<SupplierItemMappingDto>>(await _supplierRepository.GetSupplierItemMapping(supplierId, itemId, onlyEnabled));
        }

        public async Task<object> ImportSupplierAsync(string userId, IFormFile file)
        {
            if (!Utilities.IsExcelFile(file.ContentType))
            {
                throw new BadRequestException(string.Format(ErrorMessages.UnsupportedImportFile, file.ContentType));
            }

            var currentTime = DateTime.Now;
            var localFilePath =
                $"{ResourceFolder}/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";

            try
            {
                await Utilities.SaveFileToTemporaryFolder(file, file.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, file.FileName);
                var excelData = await ReadExcelSupplierDataAsync(inputFilePath);

                #region Initialize Identifier HashSet
                var supplierNo = excelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                            .Select(x => x.supplier_no!).Distinct().ToHashSet();

                var intermediarySupplierNos = excelData
                    .Where(x => !string.IsNullOrEmpty(x.intermediary_supplier_no) && !supplierNo.Contains(x.intermediary_supplier_no))
                            .Select(x => x.intermediary_supplier_no!).Distinct().ToHashSet();
                var supplierKey = new HashSet<string>(supplierNo.Union(intermediarySupplierNos));

                var siteNo = excelData.Where(x => !string.IsNullOrEmpty(x.site_no))
                    .Select(x => x.site_no!).Distinct().ToHashSet();

                var currencyCode = excelData.Where(x => !string.IsNullOrEmpty(x.default_currency))
                    .Select(x => x.default_currency!).Distinct().ToHashSet();

                var countryOfLoading = excelData.Where(x => !string.IsNullOrEmpty(x.default_country_of_loading))
                     .Select(x => x.default_country_of_loading!).Distinct().ToArray();
                var countryOfDischarge = excelData.Where(x => !string.IsNullOrEmpty(x.default_country_of_discharge))
                    .Select(x => x.default_country_of_discharge!).Distinct().ToArray();
                var countryNo = countryOfLoading.Union(countryOfDischarge).ToHashSet();

                var portOfLoading = excelData.Where(x => !string.IsNullOrEmpty(x.default_port_of_loading))
                    .Select(x => x.default_port_of_loading!).Distinct().ToArray();
                var portOfDischarge = excelData.Where(x => !string.IsNullOrEmpty(x.default_port_of_discharge))
                    .Select(x => x.default_port_of_discharge!).Distinct().ToArray();
                var portNo = portOfLoading.Union(portOfDischarge).ToHashSet();
                #endregion

                var sites = await GetSiteList(siteNo);
                var currencies = await GetCurrencyList(currencyCode);
                var lovs = await GetLovList();
                var ports = await GetPortList(portNo);
                var countries = await GetCountryList(countryNo);
                var suppliers = await GetSupplierList(supplierKey);
                var validationResult = ValidateExcelSupplierDataAsync(new ValidateSupplierRequest
                {
                    ExcelData = excelData,
                    Sites = sites,
                    Currencies = currencies,
                    Lovs = lovs,
                    Ports = ports,
                    Countries = countries,
                });

                var excelDataValid = excelData.Where(x => !validationResult.Item1.Contains(x.index)).ToList();

                if (excelDataValid.Count == 0)
                {
                    return validationResult.Item2;
                }

                var dicSuppliers = suppliers.ToDictionary(x => x.supplier_no);
                var intermediarySuppliers = await GetIntermediarySupplierList(suppliers.Select(x => x.id).ToHashSet());

                var supplierToInsert = new List<Supplier>();
                var supplierToUpdate = new List<Supplier>();
                var intermediarySuppliersToInsert = new List<IntermediarySupplier>();
                var intermediarySuppliersToUpdate = new List<IntermediarySupplier>();
                var intermediarySupplierMappings = new List<IntermediarySupplierMapping>();
                foreach (var importData in excelDataValid)
                {
                    var registeredSite = sites.FirstOrDefault(x => x.site_no == importData.site_no)?.id;
                    var countryOfLoadingId = countries.FirstOrDefault(x => x.country_alpha_code_two == importData.default_country_of_loading)?.id;
                    var portOfLoadingId = ports.FirstOrDefault(x => x.port_no == importData.default_port_of_loading)?.id;
                    var countryOfDischargeId = countries.FirstOrDefault(x => x.country_alpha_code_two == importData.default_country_of_discharge)?.id;
                    var portOfDischargeId = ports.FirstOrDefault(x => x.port_no == importData.default_port_of_discharge)?.id;
                    var currency = currencies.FirstOrDefault(x => x.currency_code == importData.default_currency)?.id;
                    if (dicSuppliers.TryGetValue(importData.supplier_no!, out var supplier))
                    {
                        // update
                        _mapper.Map(importData, supplier);
                        supplier.registered_site_id = registeredSite ?? 0;
                        supplier.default_country_of_loading_id = countryOfLoadingId ?? 0;
                        supplier.default_port_of_loading_id = portOfLoadingId ?? 0;
                        supplier.default_country_of_discharge_id = countryOfDischargeId ?? 0;
                        supplier.default_port_of_discharge_id = portOfDischargeId ?? 0;
                        supplier.default_currency_id = currency ?? 0;
                        supplier.last_modified_by = userId;
                        supplier.last_modified_on = currentTime;
                        supplierToUpdate.Add(supplier);
                    }
                    else
                    {
                        // create
                        supplier = _mapper.Map<Supplier>(importData);
                        supplier.registered_site_id = registeredSite ?? 0;
                        supplier.default_country_of_loading_id = countryOfLoadingId ?? 0;
                        supplier.default_port_of_loading_id = portOfLoadingId ?? 0;
                        supplier.default_country_of_discharge_id = countryOfDischargeId ?? 0;
                        supplier.default_port_of_discharge_id = portOfDischargeId ?? 0;
                        supplier.default_currency_id = currency ?? 0;
                        supplier.created_by = userId;
                        supplier.created_on = currentTime;
                        supplierToInsert.Add(supplier);
                    }

                    if (!string.IsNullOrEmpty(importData.intermediary_supplier_no))
                    {
                        intermediarySupplierMappings.Add(new IntermediarySupplierMapping
                        {
                            IntermediarySupplierNo = importData.intermediary_supplier_no,
                            Supplier = supplier
                        });
                    }
                }

                try
                {
                    _unitOfWork.BeginTransaction();
                    if (supplierToUpdate.Count > 0)
                    {
                        await _supplierRepository.UpdateRangeAsync(supplierToUpdate);
                    }

                    if (supplierToInsert.Count > 0)
                    {
                        await _supplierRepository.CreateRangeAsync(supplierToInsert);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    if (intermediarySupplierMappings.Count > 0)
                    {
                        var dicSupplierId =
                            supplierToInsert.Union(suppliers).ToDictionary(x => x.supplier_no, y => y.id);

                        foreach (var mapping in intermediarySupplierMappings)
                        {
                            if (!dicSupplierId.TryGetValue(mapping.IntermediarySupplierNo, out var intSupplierId))
                            {
                                //throw new BadRequestException(string.Format(ErrorMessages.IntermediarySupplierNotFound, nameof(Supplier.supplier_no), mapping.IntermediarySupplierNo));
                                _logger.LogError($"Intermediary supplier not found for supplier no: {mapping.IntermediarySupplierNo}");
                                continue;
                            }

                            var existedIntermediarySupplier = intermediarySuppliers.Find(x => x.int_supplier_id == intSupplierId && x.supplier_id == mapping.Supplier.id);

                            if (existedIntermediarySupplier == null)
                            {
                                intermediarySuppliersToInsert.Add(new IntermediarySupplier
                                {
                                    int_supplier_id = intSupplierId,
                                    supplier_id = mapping.Supplier.id,
                                    status_flag = StatusFlag.Enabled,
                                    created_by = userId,
                                    created_on = currentTime
                                });
                            }
                            else
                            {
                                existedIntermediarySupplier.supplier_id = mapping.Supplier.id;
                                existedIntermediarySupplier.int_supplier_id = intSupplierId;
                                existedIntermediarySupplier.last_modified_by = userId;
                                existedIntermediarySupplier.last_modified_on = currentTime;
                                intermediarySuppliersToUpdate.Add(existedIntermediarySupplier);
                            }
                        }

                        if (intermediarySuppliersToInsert.Count > 0)
                        {
                            await _intermediarySupplierRepository.CreateRangeAsync(intermediarySuppliersToInsert);
                        }

                        if (intermediarySuppliersToUpdate.Count > 0)
                        {
                            await _intermediarySupplierRepository.UpdateRangeAsync(intermediarySuppliersToUpdate);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    throw;
                }

                return validationResult.Item2;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
            finally
            {
                Directory.Delete(localFilePath, true);
            }
        }

        public async Task<object> ImportSupplierItemAsync(string userId, IFormFile file)
        {
            if (!Utilities.IsExcelFile(file.ContentType))
            {
                throw new BadRequestException(string.Format(ErrorMessages.UnsupportedImportFile, file.ContentType));
            }

            var currentTime = DateTime.Now;
            var localFilePath =
                $"{ResourceFolder}/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                await Utilities.SaveFileToTemporaryFolder(file, file.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, file.FileName);
                var excelData = await ReadExcelSupplierItemDataAsync(inputFilePath);

                var supplierNos = excelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                    .Select(x => x.supplier_no!).Distinct().ToHashSet();
                var suppliers = await GetSupplierList(supplierNos);

                var itemNos = excelData.Where(x => !string.IsNullOrEmpty(x.item_no))
                    .Select(x => x.item_no!).Distinct().ToHashSet();
                var items = await GetItemList(itemNos);

                var validationResult = ValidateExcelSupplierItemDataAsync(new ValidateSupplierItemMappingRequest
                {
                    ExcelData = excelData,
                    Suppliers = suppliers,
                    Items = items
                });

                var excelDataValid = excelData.Where(x => !validationResult.Item1.Contains(x.index)).ToList();

                var supplierIds = suppliers.Where(x => excelDataValid.Select(imp => imp.supplier_no).Contains(x.supplier_no))
                    .Select(x => x.id).Distinct().ToHashSet();
                var itemIds = items.Where(x => excelDataValid.Select(imp => imp.item_no).Contains(x.item_no))
                    .Select(x => x.id).Distinct().ToHashSet();
                var supplierMaterialCodes = excelDataValid.Where(x => !string.IsNullOrEmpty(x.supplier_material_code))
                    .Select(x => x.supplier_material_code).Distinct().ToHashSet();

                var dicSupplierItemMapping = await _supplierItemMappingRepository.GetDictionarySupplierItemMappingAsync(
                    supplierIds, itemIds, supplierMaterialCodes);

                var dicSupplierIds = suppliers.Distinct().ToDictionary(x => x.supplier_no);
                var dicItemIds = items.Distinct().ToDictionary(x => x.item_no);

                var supplierItemMappingToInsert = new List<SupplierItemMapping>();
                var supplierItemMappingToUpdate = new List<SupplierItemMapping>();
                foreach (var importData in excelDataValid)
                {
                    if (!dicSupplierIds.TryGetValue(importData.supplier_no!, out var supplier))
                    {
                        continue;
                    }

                    if (!dicItemIds.TryGetValue(importData.item_no!, out var item))
                    {
                        continue;
                    }

                    if (dicSupplierItemMapping.TryGetValue(new Tuple<int, int, string?>(supplier.id, item.id, importData.supplier_material_code), out var supplierItemMapping))
                    {
                        // update
                        _mapper.Map(importData, supplierItemMapping);
                        supplierItemMapping.last_modified_by = userId;
                        supplierItemMapping.last_modified_on = currentTime;
                        supplierItemMappingToUpdate.Add(supplierItemMapping);
                    }
                    else
                    {
                        // create
                        supplierItemMapping = _mapper.Map<SupplierItemMapping>(importData);
                        supplierItemMapping.supplier_id = supplier.id;
                        supplierItemMapping.item_id = item.id;
                        supplierItemMapping.created_by = userId;
                        supplierItemMapping.created_on = currentTime;
                        supplierItemMappingToInsert.Add(supplierItemMapping);
                    }
                }

                if (supplierItemMappingToUpdate.Count > 0)
                {
                    await _supplierItemMappingRepository.UpdateRangeAsync(supplierItemMappingToUpdate);
                }

                if (supplierItemMappingToInsert.Count > 0)
                {
                    await _supplierItemMappingRepository.CreateRangeAsync(supplierItemMappingToInsert);
                }

                await _unitOfWork.SaveChangesAsync();
                return validationResult.Item2;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
            finally
            {
                Directory.Delete(localFilePath, true);
            }
        }

        public async Task<object> ImportSupplierSecondaryAsync(string userId, IFormFile file)
        {
            if (!Utilities.IsExcelFile(file.ContentType))
            {
                throw new BadRequestException(string.Format(ErrorMessages.UnsupportedImportFile, file.ContentType));
            }

            var currentTime = DateTime.Now;
            var localFilePath =
                $"{ResourceFolder}/file-uploads/{currentTime.Year}/{currentTime.Month}/{currentTime.Day}/";

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                await Utilities.SaveFileToTemporaryFolder(file, file.FileName, localFilePath);
                var inputFilePath = Path.Combine(localFilePath, file.FileName);
                var excelData = await ReadExcelSupplierSecondaryDataAsync(inputFilePath);

                var supplierNos = excelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                    .Select(x => x.supplier_no!).Distinct().ToHashSet();
                var suppliers = await GetSupplierList(supplierNos);

                var validationResult = ValidateExcelSupplierSecondaryDataAsync(new ValidateSupplierSecondaryRequest
                {
                    ExcelData = excelData,
                    Suppliers = suppliers
                });

                var excelDataValid = excelData.Where(x => !validationResult.Item1.Contains(x.index)).ToList();

                var supplierSecondaryConditionMapping = suppliers.Join(excelDataValid,
                    supplier => supplier.supplier_no,
                    imp => imp.supplier_no,
                    (supplier, imp) => new Mapping
                    {
                        SupplierId = supplier.id,
                        SecondSupplierNo = imp.sec_supplier_no!
                    }).Distinct().ToHashSet();

                var dicSupplierSecondaries = await GetDictionarySupplierSecondaryAsync(supplierSecondaryConditionMapping);

                var supplierSecondaryToInsert = new List<SecondarySupplier>();
                var supplierSecondaryToUpdate = new List<SecondarySupplier>();
                foreach (var importData in excelDataValid)
                {
                    var condition =
                        supplierSecondaryConditionMapping.Where(x => x.SecondSupplierNo.Equals(importData.sec_supplier_no, StringComparison.OrdinalIgnoreCase))
                            .Select(x => new Tuple<int, string>(x.SupplierId, x.SecondSupplierNo)).FirstOrDefault();

                    if (condition is null)
                    {
                        continue;
                    }

                    if (dicSupplierSecondaries.TryGetValue(condition, out var supplierSecondary))
                    {
                        // update
                        _mapper.Map(importData, supplierSecondary);
                        supplierSecondary.last_modified_by = userId;
                        supplierSecondary.last_modified_on = currentTime;
                        supplierSecondaryToUpdate.Add(supplierSecondary);
                    }
                    else
                    {
                        // create
                        supplierSecondary = _mapper.Map<SecondarySupplier>(importData);
                        supplierSecondary.supplier_id = condition.Item1;
                        supplierSecondary.created_by = userId;
                        supplierSecondary.created_on = currentTime;
                        supplierSecondary.status_flag = ApplicationConstant.StatusFlag.Enabled;
                        supplierSecondary.default_flag = false;
                        supplierSecondaryToInsert.Add(supplierSecondary);
                    }
                }

                if (supplierSecondaryToUpdate.Count > 0)
                {
                    await _supplierSecondaryRepository.UpdateRangeAsync(supplierSecondaryToUpdate);
                }

                if (supplierSecondaryToInsert.Count > 0)
                {
                    await _supplierSecondaryRepository.CreateRangeAsync(supplierSecondaryToInsert);
                }

                await _unitOfWork.SaveChangesAsync();
                return validationResult.Item2;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
            finally
            {
                Directory.Delete(localFilePath, true);
            }
        }

        public async Task<int[]> CreateSupplierAsync(string userId, List<CreateSupplierDto> requests)
        {
            await ValidateCreateSupplierAsync(requests);
            var supplierToInsert = new List<Supplier>();
            foreach (var request in requests)
            {
                var supplier = _mapper.Map<Supplier>(request);
                supplier.created_by = userId;
                supplier.created_on = DateTime.Now;
                supplierToInsert.Add(supplier);
            }

            if (supplierToInsert.Count > 0)
            {
                await _supplierRepository.CreateRangeAsync(supplierToInsert);
                _logger.LogInformation("Created {count} supplier", supplierToInsert.Count);
                await _unitOfWork.SaveChangesAsync();
            }

            return supplierToInsert.Select(x => x.id).ToArray();
        }

        public async Task UpdateSupplierAsync(string userId, List<UpdateSupplierDto> requests)
        {
            var supplier = await GetSupplierListByIds(requests.Select(x => x.id).ToList());
            var invalidSupplierIds = requests.Select(x => x.id).Except(supplier.Select(x => x.id)).ToArray();
            if (invalidSupplierIds.Length > 0)
            {
                throw new NotFoundException(string.Format(ErrorMessages.SupplierNotFoundInDb, nameof(Supplier.id), string.Join(",", invalidSupplierIds)));
            }

            foreach (var updateSupplierDto in requests)
            {
                var supplierToUpdate = supplier.Find(x => x.id == updateSupplierDto.id);
                if (supplierToUpdate is null)
                {
                    throw new NotFoundException(string.Format(ErrorMessages.SupplierNotFoundInDb, nameof(Supplier.id), updateSupplierDto.id));
                }

                _mapper.Map(updateSupplierDto, supplierToUpdate);
                supplierToUpdate.last_modified_by = userId;
                supplierToUpdate.last_modified_on = DateTime.Now;
            }

            await ValidateUpdateSupplierAsync(supplier);

            await _supplierRepository.UpdateRangeAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(List<int> supplierIDs)
        {
            var supplier = await GetSupplierListByIds(supplierIDs);
            var invalidSupplierIds = supplierIDs.Except(supplier.Select(x => x.id)).ToArray();
            if (invalidSupplierIds.Length > 0)
            {
                throw new NotFoundException(string.Format(ErrorMessages.SupplierNotFoundInDb, nameof(Supplier.id), string.Join(",", invalidSupplierIds)));
            }

            // TODO: Delete intermediary supplier
            // TODO: Delete secondary supplier

            await _supplierRepository.DeleteRangeAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<PagedResponse<SupplierPagedResponseDto>> SearchPagedAsync(SearchPagedRequestDto request, FilterPagedSupplierRequestDto filter)
        {
            var query = _supplierRepository.BuildSupplierFilterQuery(new PagedFilterSupplierRequestModel
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                service_flag = filter.service_flag,
                product_flag = filter.product_flag,
                status_flag = filter.status_flag
            }, out var totalRows);

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.Supplier,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<PagedSupplierDetail>(listSort);

            if (totalRows == 0)
            {
                return Task.FromResult(new PagedResponse<SupplierPagedResponseDto>());
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            return Task.FromResult(new PagedResponse<SupplierPagedResponseDto>
            {
                Items = _mapper.Map<List<SupplierPagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            });
        }

        public Task<PagedResponse<SupplierItemMappingPagedResponseDto>> SearchItemMappingPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter)
        {
            var query = _supplierItemMappingRepository.BuildSupplierItemMappingFilterQuery(new FilterModel
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                default_flag = filter.default_flag,
                status_flag = filter.status_flag,
                supplier_id = filter.supplier_id
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.SupplierItemMapping,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<SupplierItemMappingPagedResponseDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return Task.FromResult(new PagedResponse<SupplierItemMappingPagedResponseDto>());
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            return Task.FromResult(new PagedResponse<SupplierItemMappingPagedResponseDto>
            {
                Items = _mapper.Map<List<SupplierItemMappingPagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            });
        }

        public Task<PagedResponse<SecondarySupplierPagedResponseDto>> SearchSecondaryPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter)
        {
            var query = _supplierSecondaryRepository.BuildSupplierSecondaryFilterQuery(new FilterModel
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                default_flag = filter.default_flag,
                status_flag = filter.status_flag,
                supplier_id = filter.supplier_id
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.SupplierSecondary,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<SupplierSecondaryPagedResponseDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return Task.FromResult(new PagedResponse<SecondarySupplierPagedResponseDto>());
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            return Task.FromResult(new PagedResponse<SecondarySupplierPagedResponseDto>
            {
                Items = _mapper.Map<List<SecondarySupplierPagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            });
        }

        public Task<PagedResponse<IntermediarySupplierPagedResponseDto>> SearchIntermediaryPagedAsync(SearchPagedRequestDto request, FilterPagedRelativeSupplierRequestDto filter)
        {
            var query = _intermediarySupplierRepository.BuildSupplierIntermediaryFilterQuery(new FilterModel
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                default_flag = filter.default_flag,
                status_flag = filter.status_flag,
                supplier_id = filter.supplier_id
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.SupplierIntermediary,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<IntermediarySupplierPagedResponseDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return Task.FromResult(new PagedResponse<IntermediarySupplierPagedResponseDto>());
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            return Task.FromResult(new PagedResponse<IntermediarySupplierPagedResponseDto>
            {
                Items = _mapper.Map<List<IntermediarySupplierPagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            });
        }

        public Task<PagedResponse<SupplierSelfCollectSitePagedResponseDto>> SearchSelfCollectSitePagedAsync(SearchPagedRequestDto request, FilterPagedSupplierSelfCollectSiteRequestDto filter)
        {
            var query = _supplierSelfCollectSiteRepository.BuildSelfCollectSiteFilterQuery(new FilterModel
            {
                Keyword = request.Keyword,
                create_date_from = filter.create_date_from,
                create_date_to = filter.create_date_to,
                status_flag = filter.status_flag,
                supplier_id = filter.supplier_id
            });

            var listSort = new List<Sortable>
            {
                new()
                {
                    FieldName = request.SortBy ?? DefaultSortField.SupplierSelfCollectSite,
                    IsAscending = request.SortAscending
                }
            };

            var orderBy = ApplySort.GetOrderByFunction<SupplierSelfCollectSitePagedResponseDetail>(listSort);

            var totalRows = query.Count();
            if (totalRows == 0)
            {
                return Task.FromResult(new PagedResponse<SupplierSelfCollectSitePagedResponseDto>());
            }

            var pageable = PagingUtilities.GetPageable(request.Page, request.PageSize);
            var skipRow = PagingUtilities.GetSkipRow(pageable.Page, pageable.Size);
            var totalPage = (int)Math.Ceiling(totalRows / (double)pageable.Size);

            var pagedResponse = orderBy(query)
                .Skip(skipRow)
                .Take(pageable.Size)
                .ToList();

            return Task.FromResult(new PagedResponse<SupplierSelfCollectSitePagedResponseDto>
            {
                Items = _mapper.Map<List<SupplierSelfCollectSitePagedResponseDto>>(pagedResponse),
                TotalItems = totalRows,
                TotalPage = totalPage,
                Page = pageable.Page,
                PageSize = pageable.Size
            });
        }

        public async Task UpdateItemMappingAsync(string userId, int suppplierId, List<UpdateItemMappingRequestDto> requests)
        {
            var supplierMappingItemIds = requests.Select(x => x.supplier_item_mapping_id).ToList();
            var supplierMappingItems = await GetSupplierItemMappingListAsync(supplierMappingItemIds);

            var supplierMappingItemToInsert = new List<SupplierItemMapping>();
            var supplierMappingItemToUpdate = new List<SupplierItemMapping>();
            var supplierMappingItemToDelete = new List<SupplierItemMapping>();

            foreach (var requestDto in requests)
            {
                var supplierMappingItem = supplierMappingItems.Find(x => x.id == requestDto.supplier_item_mapping_id);

                switch (requestDto.action)
                {
                    case ActionFlag.Create:
                        {
                            var supplierItemMapping = new SupplierItemMapping
                            {
                                supplier_id = suppplierId,
                                status_flag = requestDto.status_flag,
                                default_flag = requestDto.default_flag,
                                item_id = requestDto.item_id,
                                supplier_part_no = requestDto.supplier_part_no,
                                supplier_material_code = requestDto.supplier_material_code,
                                supplier_material_description = requestDto.supplier_material_description,
                                created_by = userId,
                                created_on = DateTime.Now
                            };
                            supplierMappingItemToInsert.Add(supplierItemMapping);
                            break;
                        }
                    case ActionFlag.Update:
                        {
                            if (supplierMappingItem == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierMappingItemNotFoundInDb, nameof(supplierMappingItem.id), requestDto.supplier_item_mapping_id));
                            }

                            supplierMappingItem.supplier_id = suppplierId;
                            supplierMappingItem.status_flag = requestDto.status_flag;
                            supplierMappingItem.default_flag = requestDto.default_flag;
                            supplierMappingItem.item_id = requestDto.item_id;
                            supplierMappingItem.supplier_part_no = requestDto.supplier_part_no;
                            supplierMappingItem.supplier_material_code = requestDto.supplier_material_code;
                            supplierMappingItem.supplier_material_description = requestDto.supplier_material_description;
                            supplierMappingItem.last_modified_by = userId;
                            supplierMappingItem.last_modified_on = DateTime.Now;
                            supplierMappingItemToUpdate.Add(supplierMappingItem);
                            break;
                        }
                    case ActionFlag.Delete:
                        {
                            if (supplierMappingItem == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierMappingItemNotFoundInDb, nameof(supplierMappingItem.id), requestDto.supplier_item_mapping_id));
                            }

                            supplierMappingItemToDelete.Add(supplierMappingItem);
                            break;
                        }
                }
            }

            try
            {
                if (supplierMappingItemToInsert.Count > 0)
                {
                    await _supplierItemMappingRepository.CreateRangeAsync(supplierMappingItemToInsert);
                    _logger.LogInformation($"Supplier item mapping created {supplierMappingItemToInsert.Count} successfully.");
                }

                if (supplierMappingItemToUpdate.Count > 0)
                {
                    await _supplierItemMappingRepository.UpdateRangeAsync(supplierMappingItemToUpdate);
                    _logger.LogInformation($"Supplier item mapping updated {supplierMappingItemToUpdate.Count} successfully.");
                }

                if (supplierMappingItemToDelete.Count > 0)
                {
                    await _supplierItemMappingRepository.DeleteRangeAsync(supplierMappingItemToDelete);
                    _logger.LogInformation($"Supplier item mapping deleted {supplierMappingItemToDelete.Count} successfully.");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateSecondaryAsync(string userId, int suppplierId, List<UpdateSecondarySupplierRequestDto> requests)
        {
            var supplierSecondaries = await GetSupplierSecondaryListAsync(requests.Select(x => x.id).ToList());

            var supplierSecondaryToInsert = new List<SecondarySupplier>();
            var supplierSecondaryToUpdate = new List<SecondarySupplier>();
            var supplierSecondaryToDelete = new List<SecondarySupplier>();
            foreach (var requestDto in requests)
            {
                var secondarySupplier = supplierSecondaries.Find(x => x.id == requestDto.id);
                switch (requestDto.action)
                {
                    case ActionFlag.Create:
                        {
                            var supplierSecondary = new SecondarySupplier
                            {
                                supplier_id = suppplierId,
                                status_flag = requestDto.status_flag,
                                sec_supplier_no = requestDto.secondary_supplier_no,
                                sec_supplier_name = requestDto.secondary_supplier_name,
                                default_flag = requestDto.default_flag,
                                created_by = userId,
                                created_on = DateTime.Now
                            };

                            supplierSecondaryToInsert.Add(supplierSecondary);
                            break;
                        }
                    case ActionFlag.Update:
                        {
                            if (secondarySupplier == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierSecondaryNotFoundInDb, nameof(secondarySupplier.id), requestDto.id));
                            }

                            secondarySupplier.supplier_id = suppplierId;
                            secondarySupplier.sec_supplier_no = requestDto.secondary_supplier_no;
                            secondarySupplier.sec_supplier_name = requestDto.secondary_supplier_name;
                            secondarySupplier.status_flag = requestDto.status_flag;
                            secondarySupplier.default_flag = requestDto.default_flag;
                            secondarySupplier.last_modified_by = userId;
                            secondarySupplier.last_modified_on = DateTime.Now;
                            supplierSecondaryToUpdate.Add(secondarySupplier);
                            break;
                        }

                    case ActionFlag.Delete:
                        {
                            if (secondarySupplier == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierSecondaryNotFoundInDb, nameof(secondarySupplier.id), requestDto.id));
                            }

                            supplierSecondaryToDelete.Add(secondarySupplier);
                            break;
                        }
                }
            }

            try
            {
                if (supplierSecondaryToInsert.Count > 0)
                {
                    await _supplierSecondaryRepository.CreateRangeAsync(supplierSecondaryToInsert);
                    _logger.LogInformation($"Supplier secondary created {supplierSecondaryToInsert.Count} successfully.");
                }

                if (supplierSecondaryToUpdate.Count > 0)
                {
                    await _supplierSecondaryRepository.UpdateRangeAsync(supplierSecondaryToUpdate);
                    _logger.LogInformation($"Supplier secondary updated {supplierSecondaryToUpdate.Count} successfully.");
                }

                if (supplierSecondaryToDelete.Count > 0)
                {
                    await _supplierSecondaryRepository.DeleteRangeAsync(supplierSecondaryToDelete);
                    _logger.LogInformation($"Supplier secondary deleted {supplierSecondaryToDelete.Count} successfully.");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateIntermediaryAsync(string userId, int suppplierId, List<UpdateIntermediaryRequestDto> request)
        {
            var supplierIntermediaries =
                await GetSupplierIntermediaryListAsync(request.Select(x => x.id).ToList());

            var supplierIntermediaryToInsert = new List<IntermediarySupplier>();
            var supplierIntermediaryToUpdate = new List<IntermediarySupplier>();
            var supplierIntermediaryToDelete = new List<IntermediarySupplier>();
            foreach (var requestDto in request)
            {
                switch (requestDto.action)
                {
                    case ActionFlag.Create:
                        {
                            var intermediarySupplier = new IntermediarySupplier
                            {
                                status_flag = requestDto.status_flag,
                                int_supplier_id = requestDto.intermediary_supplier_id,
                                supplier_id = suppplierId,
                                default_flag = requestDto.default_flag,
                                created_by = userId,
                                created_on = DateTime.Now
                            };

                            supplierIntermediaryToInsert.Add(intermediarySupplier);
                            break;
                        }
                    case ActionFlag.Update:
                        {
                            var intermediarySupplier =
                                supplierIntermediaries.Find(x => x.id == requestDto.id);

                            if (intermediarySupplier == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierIntermediaryNotFoundInDb,
                                    nameof(intermediarySupplier.id), requestDto.intermediary_supplier_id));
                            }

                            intermediarySupplier.supplier_id = suppplierId;
                            intermediarySupplier.int_supplier_id = requestDto.intermediary_supplier_id;
                            intermediarySupplier.status_flag = requestDto.status_flag;
                            intermediarySupplier.default_flag = requestDto.default_flag;
                            intermediarySupplier.last_modified_by = userId;
                            intermediarySupplier.last_modified_on = DateTime.Now;
                            supplierIntermediaryToUpdate.Add(intermediarySupplier);
                            break;
                        }
                    case ActionFlag.Delete:
                        {
                            var intermediarySupplier =
                                supplierIntermediaries.Find(x => x.id == requestDto.id);

                            if (intermediarySupplier == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierIntermediaryNotFoundInDb,
                                    nameof(intermediarySupplier.id), requestDto.intermediary_supplier_id));
                            }

                            supplierIntermediaryToDelete.Add(intermediarySupplier);
                            break;
                        }
                }
            }

            try
            {
                if (supplierIntermediaryToInsert.Count > 0)
                {
                    await _intermediarySupplierRepository.CreateRangeAsync(supplierIntermediaryToInsert);
                    _logger.LogInformation($"Supplier intermediary created {supplierIntermediaryToInsert.Count} successfully.");
                }

                if (supplierIntermediaryToUpdate.Count > 0)
                {
                    await _intermediarySupplierRepository.UpdateRangeAsync(supplierIntermediaryToUpdate);
                    _logger.LogInformation($"Supplier intermediary updated {supplierIntermediaryToUpdate.Count} successfully.");
                }

                if (supplierIntermediaryToDelete.Count > 0)
                {
                    await _intermediarySupplierRepository.DeleteRangeAsync(supplierIntermediaryToDelete);
                    _logger.LogInformation($"Supplier intermediary deleted {supplierIntermediaryToDelete.Count} successfully.");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateSelfCollectSiteAsync(string userId, int supplierId, List<UpdateSupplierSelfCollectSiteRequestDto> request)
        {
            var supplierCollectSites =
                await GetSupplierSelfCollectSiteListAsync(request.Select(x => x.id).ToList());

            var supplierSelfCollectSiteToInsert = new List<SupplierSelfCollectSite>();
            var supplierSelfCollectSiteToUpdate = new List<SupplierSelfCollectSite>();
            var supplierSelfCollectSiteToDelete = new List<SupplierSelfCollectSite>();
            foreach (var requestDto in request)
            {
                switch (requestDto.action)
                {
                    case ActionFlag.Create:
                        {
                            var supplierSelfCollectSite = new SupplierSelfCollectSite
                            {
                                supplier_id = supplierId,
                                site_id = requestDto.self_collect_site_id,
                                status_flag = requestDto.status_flag,
                                created_by = userId,
                                created_on = DateTime.Now
                            };

                            supplierSelfCollectSiteToInsert.Add(supplierSelfCollectSite);
                            break;
                        }
                    case ActionFlag.Update:
                        {
                            var supplierSelfCollectSite =
                                supplierCollectSites.Find(x => x.id == requestDto.id);

                            if (supplierSelfCollectSite == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierSelfCollectSiteNotFoundInDb,
                                    nameof(supplierSelfCollectSite.id), requestDto.self_collect_site_id));
                            }

                            supplierSelfCollectSite.supplier_id = supplierId;
                            supplierSelfCollectSite.site_id = requestDto.self_collect_site_id;
                            supplierSelfCollectSite.status_flag = requestDto.status_flag;
                            supplierSelfCollectSite.last_modified_by = userId;
                            supplierSelfCollectSite.last_modified_on = DateTime.Now;
                            supplierSelfCollectSiteToUpdate.Add(supplierSelfCollectSite);
                            break;
                        }
                    case ActionFlag.Delete:
                        {
                            var supplierSelfCollectSite =
                                supplierCollectSites.Find(x => x.id == requestDto.id);

                            if (supplierSelfCollectSite == null)
                            {
                                throw new NotFoundException(string.Format(ErrorMessages.SupplierSelfCollectSiteNotFoundInDb,
                                    nameof(supplierSelfCollectSite.id), requestDto.self_collect_site_id));
                            }

                            supplierSelfCollectSiteToDelete.Add(supplierSelfCollectSite);
                            break;
                        }
                }
            }

            try
            {
                if (supplierSelfCollectSiteToInsert.Count > 0)
                {
                    await _supplierSelfCollectSiteRepository.CreateRangeAsync(supplierSelfCollectSiteToInsert);
                    _logger.LogInformation($"Supplier self collect site created {supplierSelfCollectSiteToInsert.Count} successfully.");
                }

                if (supplierSelfCollectSiteToUpdate.Count > 0)
                {
                    await _supplierSelfCollectSiteRepository.UpdateRangeAsync(supplierSelfCollectSiteToUpdate);
                    _logger.LogInformation($"Supplier self collect site updated {supplierSelfCollectSiteToUpdate.Count} successfully.");
                }

                if (supplierSelfCollectSiteToDelete.Count > 0)
                {
                    await _supplierSelfCollectSiteRepository.DeleteRangeAsync(supplierSelfCollectSiteToDelete);
                    _logger.LogInformation($"Supplier self collect site deleted {supplierSelfCollectSiteToDelete.Count} successfully.");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #region Private Methods
        private async Task<List<SupplierItemMapping>> GetSupplierItemMappingListAsync(List<int> supplierMappingItemIds)
        {
            var supplierMappingItem = await _supplierItemMappingRepository.Find(x => supplierMappingItemIds.Contains(x.id));
            return supplierMappingItem.ToList();
        }

        private async Task<List<SecondarySupplier>> GetSupplierSecondaryListAsync(List<int> supplierSecondaryIds)
        {
            var supplierSecondary = await _supplierSecondaryRepository.Find(x => supplierSecondaryIds.Contains(x.id));
            return supplierSecondary.ToList();
        }

        private async Task<List<IntermediarySupplier>> GetSupplierIntermediaryListAsync(List<int> supplierIntermediaryIds)
        {
            var supplierIntermediary = await _intermediarySupplierRepository.Find(x => supplierIntermediaryIds.Contains(x.id));
            return supplierIntermediary.ToList();
        }

        private async Task<List<SupplierSelfCollectSite>> GetSupplierSelfCollectSiteListAsync(List<int> supplierSelfCollectSiteIds)
        {
            var supplierSelfCollectSite = await _supplierSelfCollectSiteRepository.Find(x => supplierSelfCollectSiteIds.Contains(x.id));
            return supplierSelfCollectSite.ToList();
        }

        private class Mapping
        {
            public int SupplierId { get; set; }
            public string SecondSupplierNo { get; set; }
        }

        private async Task ValidateCreateSupplierAsync(List<CreateSupplierDto> requestDto)
        {
            // supplier_no must not exist in table Supplier
            var supplierNos = requestDto.Select(x => x.supplier_no).Distinct().ToList();
            var invalidSupplierNos = await _supplierRepository.CheckExistingSupplierNo(supplierNos);
            if (invalidSupplierNos.Length > 0)
            {
                throw new BadRequestException(string.Format(ErrorMessages.SupplierAlreadyExists, nameof(Supplier.supplier_no), string.Join(",", invalidSupplierNos)));
            }

            await ValidateSupplierBasedLovDataAsync(_mapper.Map<List<Supplier>>(requestDto));
        }

        private async Task ValidateSupplierBasedLovDataAsync(List<Supplier> supplier)
        {
            var lovData = await GetLovList();

            // - PO Sending Method in LOV POSendMethods
            #region PoSendingMethod
            var poSendMethods = lovData.Where(x => x.lov_type == LOVs.Type.PoSendMethod).Select(x => x.lov_value)
                    .Distinct().ToHashSet();

            if (poSendMethods.Count > 0)
            {
                var invalidPoSendingMethods = supplier.Where(x => !string.IsNullOrEmpty(x.po_sending_method))
                    .Select(x => x.po_sending_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.PoSendMethod).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                if (invalidPoSendingMethods.Count > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.po_sending_method), string.Join(",", invalidPoSendingMethods),
                        LOVs.Type.PoSendMethod));

                }
            }
            #endregion

            // - Payment Term in LOV PaymentTerms
            #region PaymentTerm
            var paymentTerms = lovData.Where(x => x.lov_type == LOVs.Type.PaymentTerm).Select(x => x.lov_value)
                    .Distinct().ToHashSet();

            if (paymentTerms.Count > 0)
            {
                var invalidPaymentTerms = supplier.Where(x => !string.IsNullOrEmpty(x.payment_term))
                    .Select(x => x.payment_term).Except(lovData.Where(x => x.lov_type == LOVs.Type.PaymentTerm)
                        .Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                if (invalidPaymentTerms.Count > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.payment_term), string.Join(",", invalidPaymentTerms), LOVs.Type.PaymentTerm));
                }
            }
            #endregion

            // - Cost Rule in LOV CostRules
            #region CostRule
            var costRules = lovData.Where(x => x.lov_type == LOVs.Type.CostRule).Select(x => x.lov_value).Distinct().ToHashSet();
            if (costRules.Count > 0)
            {
                var invalidCostRules = supplier.Where(x => !string.IsNullOrEmpty(x.landed_cost_rule))
                    .Select(x => x.landed_cost_rule).Except(lovData.Where(x => x.lov_type == LOVs.Type.CostRule).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                if (invalidCostRules.Count > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.landed_cost_rule), string.Join(",", invalidCostRules), LOVs.Type.CostRule));
                }
            }
            #endregion

            // - Inco Term in LOV IncoTerms
            #region IncoTerm
            var incoTerms = lovData.Where(x => x.lov_type == LOVs.Type.IncoTerm).Select(x => x.lov_value).Distinct().ToHashSet();
            if (incoTerms.Count > 0)
            {
                var invalidIncoTerms = supplier.Where(x => !string.IsNullOrEmpty(x.incoterm))
                    .Select(x => x.incoterm).Except(lovData.Where(x => x.lov_type == LOVs.Type.IncoTerm).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                if (invalidIncoTerms.Count > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.incoterm), string.Join(",", invalidIncoTerms), LOVs.Type.IncoTerm));
                }
            }
            #endregion

            // - Default Freight Method in LOV FreightMethods
            #region FreightMethod
            var freightMethods = lovData.Where(x => x.lov_type == LOVs.Type.FreightMethod).Select(x => x.lov_value).Distinct().ToHashSet();

            if (freightMethods.Count > 0)
            {
                var invalidFreightMethods = supplier.Where(x => !string.IsNullOrEmpty(x.default_freight_method))
                    .Select(x => x.default_freight_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.FreightMethod).Select(x => x.lov_value))
                    .Distinct().ToArray();

                if (invalidFreightMethods.Length > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.default_freight_method), string.Join(",", invalidFreightMethods), LOVs.Type.FreightMethod));
                }
            }
            #endregion

            #region PaymentMethod
            var paymentMethods = lovData.Where(x => x.lov_type == LOVs.Type.PaymentMethods).Select(x => x.lov_value).Distinct().ToHashSet();

            if (paymentMethods.Count > 0)
            {
                var invalidPaymentMethods = supplier.Where(x => !string.IsNullOrEmpty(x.payment_method))
                    .Select(x => x.payment_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.PaymentMethods).Select(x => x.lov_value))
                    .Distinct().ToArray();

                if (invalidPaymentMethods.Length > 0)
                {
                    throw new BadRequestException(string.Format(ErrorMessages.ValueMustExistingInLov,
                        nameof(Supplier.payment_method), string.Join(",", invalidPaymentMethods), LOVs.Type.PaymentMethods));
                }
            }
            #endregion

            var currencyIds = supplier.Select(x => x.default_currency_id ?? 0)
                .Distinct().ToHashSet();
            var availableCurrency = await _currencyRepository.GetCurrencyAvailable(currencyIds);
            var invalidCurrencyId = currencyIds.Except(availableCurrency).ToArray();
            if (invalidCurrencyId.Length > 0)
            {
                throw new BadRequestException(string.Format(ErrorMessages.SupplierDefaultCurrencyNotFound,
                    string.Join(",", invalidCurrencyId)));
            }

            var registeredSiteIds = supplier.Select(x => x.registered_site_id)
                .Distinct().ToHashSet();
            var availableSite = await _siteRepository.GetSiteAvailable(registeredSiteIds);
            var invalidSiteId = registeredSiteIds.Except(availableSite).ToArray();
            if (invalidSiteId.Length > 0)
            {
                throw new BadRequestException(string.Format(ErrorMessages.RegisteredSiteNotFound,
                    string.Join(",", invalidSiteId)));
            }
        }

        private async Task<Dictionary<Tuple<int, string>, SecondarySupplier>> GetDictionarySupplierSecondaryAsync(HashSet<Mapping> supplierSecondaryConditionMapping)
        {
            var parameter = Expression.Parameter(typeof(SecondarySupplier), "x");
            Expression predicate = Expression.Constant(false);

            foreach (var condition in supplierSecondaryConditionMapping)
            {
                var supplierIdCondition = Expression.Equal(
                    Expression.Property(parameter, "supplier_id"),
                    Expression.Constant(condition.SupplierId)
                );

                var secSupplierNoCondition = Expression.Equal(
                    Expression.Property(parameter, "sec_supplier_no"),
                    Expression.Constant(condition.SecondSupplierNo)
                );

                var combinedCondition = Expression.AndAlso(supplierIdCondition, secSupplierNoCondition);
                predicate = Expression.OrElse(predicate, combinedCondition);
            }

            var lambda = Expression.Lambda<Func<SecondarySupplier, bool>>(predicate, parameter);

            var dicSupplierSecondaries = await _supplierSecondaryRepository.GetDictionarySupplierSecondaryAsync(lambda);
            return dicSupplierSecondaries;
        }

        private async Task<List<IntermediarySupplier>> GetIntermediarySupplierList(HashSet<int> supplierIds)
        {
            if (supplierIds.Count > 0)
            {
                return [];
            }

            var intermediarySuppliers = await _supplierRepository.GetIntermediarySupplierList(supplierIds);
            return intermediarySuppliers;
        }

        private async Task<List<Country>> GetCountryList(HashSet<string> countryNo)
        {
            if (countryNo.Count == 0)
            {
                return [];
            }

            var countries = await _countryRepository.GetCountryCodeListAsync(countryNo);

            return countries;
        }

        private async Task<List<Port>> GetPortList(HashSet<string> portNo)
        {
            if (portNo.Count == 0)
            {
                return [];
            }

            var ports = await _portRepository.GetPortListAsync(portNo);

            return ports;
        }

        private async Task<List<Item>> GetItemList(HashSet<string>? itemNos)
        {
            if (itemNos is null)
            {
                return [];
            }

            var items = await _itemRepository.GetItemListAsync(itemNos);

            return items;
        }

        private async Task<List<Supplier>> GetSupplierList(HashSet<string>? supplierNos)
        {
            if (supplierNos is null)
            {
                return [];
            }

            var suppliers = await _supplierRepository.Find(x => supplierNos.Contains(x.supplier_no));

            return suppliers.ToList();
        }

        private async Task<List<Supplier>> GetSupplierListByIds(List<int> supplierIds)
        {
            var suppliers = await _supplierRepository.Find(x => supplierIds.Contains(x.id));
            return suppliers.ToList();
        }

        private async Task<List<Site>> GetSiteList(HashSet<string> siteNo)
        {
            if (siteNo.Count == 0)
            {
                return [];
            }

            var sites = await _siteRepository.GetSiteNoListAsync(siteNo);

            return sites;
        }

        private async Task<List<Currency>> GetCurrencyList(HashSet<string> currencyCode)
        {
            if (currencyCode.Count == 0)
            {
                return [];
            }

            var currencies = await _currencyRepository.GetCurrencyListAsync(currencyCode);
            return currencies;
        }

        private async Task<List<Lov>> GetLovList()
        {
            var lov = await _lovRepository.GetByLovTypeAsync(
            [
                LOVs.Type.PoSendMethod,
                LOVs.Type.PaymentTerm,
                LOVs.Type.CostRule,
                LOVs.Type.IncoTerm,
                LOVs.Type.FreightMethod,
                LOVs.Type.PaymentMethods
            ], onlyEnabled: true);

            return lov.ToList();
        }

        /// <summary>
        /// Read supplier's data from excel file
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>
        private async Task<List<ImportSupplierData>> ReadExcelSupplierDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportSupplierData>();
            await using var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = a => new ExcelDataTableConfiguration
                {
                    // Gets or sets a value indicating whether to use a row from the 
                    // data as column names.
                    UseHeaderRow = true,
                }
            };

            // 2. Use the AsDataSet extension method
            var dataSet = reader.AsDataSet(conf);
            var dataTable = dataSet.Tables[FirstTable];
            var rowIndex = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var rowData = new ImportSupplierData
                {
                    index = rowIndex++,
                    supplier_no = item[SupplierExcelColumns.SupplierNo] != DBNull.Value
                        ? item[SupplierExcelColumns.SupplierNo].ToString()
                        : null,
                    supplier_name = item[SupplierExcelColumns.SupplierName] != DBNull.Value
                        ? item[SupplierExcelColumns.SupplierName].ToString()
                        : null,
                    status_flag = item[SupplierExcelColumns.StatusFlag] != DBNull.Value
                        ? (item[SupplierExcelColumns.StatusFlag]).ToString()
                        : null,
                    po_sending_method = item[SupplierExcelColumns.POSendingMethod] != DBNull.Value
                        ? (item[SupplierExcelColumns.POSendingMethod]).ToString()
                        : null,
                    authorised_flag = item[SupplierExcelColumns.AuthorisedFlag] != DBNull.Value
                        ? Utilities.ParseBool(item[SupplierExcelColumns.AuthorisedFlag].ToString())
                        : null,
                    payment_term = item[SupplierExcelColumns.PaymentTerm] != DBNull.Value
                        ? (item[SupplierExcelColumns.PaymentTerm]).ToString()
                        : null,
                    payment_method = item[SupplierExcelColumns.PaymentMethod] != DBNull.Value
                        ? (item[SupplierExcelColumns.PaymentMethod]).ToString()
                        : null,
                    default_currency = item[SupplierExcelColumns.DefaultCurrency] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultCurrency]).ToString()
                        : null,
                    cost_rule = item[SupplierExcelColumns.CostRule] != DBNull.Value
                        ? (item[SupplierExcelColumns.CostRule]).ToString()
                        : null,
                    inco_term = item[SupplierExcelColumns.IncoTerm] != DBNull.Value
                        ? (item[SupplierExcelColumns.IncoTerm]).ToString()
                        : null,
                    product_flag = item[SupplierExcelColumns.ProductFlag] != DBNull.Value
                        ? Utilities.ParseBool(item[SupplierExcelColumns.ProductFlag].ToString())
                        : null,
                    intermediary_supplier_no = item[SupplierExcelColumns.IntermediarySupplierNo] != DBNull.Value
                        ? (item[SupplierExcelColumns.IntermediarySupplierNo]).ToString()
                        : null,
                    default_freight_method = item[SupplierExcelColumns.DefaultFreightMethod] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultFreightMethod]).ToString()
                        : null,
                    default_country_of_loading = item[SupplierExcelColumns.DefaultCountryOfLoading] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultCountryOfLoading]).ToString()
                        : null,
                    default_port_of_loading = item[SupplierExcelColumns.DefaultPortOfLoading] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultPortOfLoading]).ToString()
                        : null,
                    default_country_of_discharge = item[SupplierExcelColumns.DefaultCountryOfDischarge] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultCountryOfDischarge]).ToString()
                        : null,
                    default_port_of_discharge = item[SupplierExcelColumns.DefaultPortOfDischarge] != DBNull.Value
                        ? (item[SupplierExcelColumns.DefaultPortOfDischarge]).ToString()
                        : null,
                    site_no = item[SupplierExcelColumns.SiteNo] != DBNull.Value
                        ? (item[SupplierExcelColumns.SiteNo]).ToString()
                        : null
                };

                // prevent null values
                var attributeTypeValueFile = Utilities.GetPropertyNameAndValues(rowData, excludedProperties);
                if (attributeTypeValueFile.All(x => x.Value is null))
                {
                    continue;
                }

                result.Add(rowData);
            }

            return result;
        }

        /// <summary>
        /// Read supplier item mapping data from excel file
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>
        private async Task<List<ImportSupplierItemData>> ReadExcelSupplierItemDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportSupplierItemData>();
            await using var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = a => new ExcelDataTableConfiguration
                {
                    // Gets or sets a value indicating whether to use a row from the 
                    // data as column names.
                    UseHeaderRow = true,
                }
            };

            // 2. Use the AsDataSet extension method
            var dataSet = reader.AsDataSet(conf);
            var dataTable = dataSet.Tables[FirstTable];
            var rowIndex = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var rowData = new ImportSupplierItemData
                {
                    index = rowIndex++,
                    supplier_no = item[SupplierItemExcelColumns.SupplierNo] != DBNull.Value
                        ? item[SupplierItemExcelColumns.SupplierNo].ToString()
                        : null,

                    item_no = item[SupplierItemExcelColumns.ItemNo] != DBNull.Value
                        ? item[SupplierItemExcelColumns.ItemNo].ToString()
                        : null,

                    supplier_part_no = item[SupplierItemExcelColumns.SupplierPartNumber] != DBNull.Value
                        ? item[SupplierItemExcelColumns.SupplierPartNumber].ToString()
                        : null,

                    supplier_material_code = item[SupplierItemExcelColumns.SupplierMaterialCode] != DBNull.Value
                        ? item[SupplierItemExcelColumns.SupplierMaterialCode].ToString()
                        : null,

                    supplier_material_description =
                        item[SupplierItemExcelColumns.SupplierMaterialDescription] != DBNull.Value
                            ? item[SupplierItemExcelColumns.SupplierMaterialDescription].ToString()
                            : null,

                    default_flag = item[SupplierItemExcelColumns.DefaultFlag] != DBNull.Value
                        ? Utilities.ParseBool(item[SupplierItemExcelColumns.DefaultFlag].ToString())
                        : null,

                    status_flag = item[SupplierItemExcelColumns.StatusFlag] != DBNull.Value
                        ? item[SupplierItemExcelColumns.StatusFlag].ToString()
                        : null
                };

                // prevent null values
                var attributeTypeValueFile = Utilities.GetPropertyNameAndValues(rowData, excludedProperties);
                if (attributeTypeValueFile.All(x => x.Value is null))
                {
                    continue;
                }

                result.Add(rowData);
            }

            return result;
        }

        /// <summary>
        /// Read supplier secondary data
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<ImportSupplierSecondaryData>> ReadExcelSupplierSecondaryDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportSupplierSecondaryData>();
            await using var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = a => new ExcelDataTableConfiguration
                {
                    // Gets or sets a value indicating whether to use a row from the 
                    // data as column names.
                    UseHeaderRow = true,
                }
            };

            // 2. Use the AsDataSet extension method
            var dataSet = reader.AsDataSet(conf);
            var dataTable = dataSet.Tables[FirstTable];
            var rowIndex = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var rowData = new ImportSupplierSecondaryData
                {
                    index = rowIndex++,

                    supplier_no = item[SupplierSecondaryExcelColumns.SupplierNo] != DBNull.Value
                        ? item[SupplierSecondaryExcelColumns.SupplierNo].ToString()
                        : null,

                    sec_supplier_no = item[SupplierSecondaryExcelColumns.SecondarySupplierNo] != DBNull.Value
                        ? item[SupplierSecondaryExcelColumns.SecondarySupplierNo].ToString()
                        : null,

                    sec_supplier_name = item[SupplierSecondaryExcelColumns.SecondarySupplierName] != DBNull.Value
                        ? item[SupplierSecondaryExcelColumns.SecondarySupplierName].ToString()
                        : null
                };

                // prevent null values
                var attributeTypeValueFile = Utilities.GetPropertyNameAndValues(rowData, excludedProperties);
                if (attributeTypeValueFile.All(x => x.Value is null))
                {
                    continue;
                }

                result.Add(rowData);
            }

            return result;
        }

        /// <summary>
        /// Validate excel supplier's data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelSupplierDataAsync(ValidateSupplierRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region SupplierNo
            // missing supplier_no
            var missingSupplierNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.supplier_no)).ToList();

            if (missingSupplierNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierData.supplier_no), string.Join(",", missingSupplierNo.Select(x => x.index)))
                });
                invalidRows.AddRange(missingSupplierNo.Select(x => x.index).ToArray());
            }

            // duplicate supplier_no
            var duplicatedSupplierNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                .GroupBy(x => x.supplier_no).Where(x => x.Count() > 1).ToList();

            if (duplicatedSupplierNo.Count > 0)
            {
                foreach (var invalidItems in duplicatedSupplierNo)
                {
                    report.Add(new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.DuplicateField, nameof(ImportSupplierData.supplier_no), string.Join(",", invalidItems.Select(x => x.index)))
                    });
                    invalidRows.AddRange(invalidItems.Select(x => x.index).ToArray());
                }
            }

            // supplier no too long
            var maxLengthOfISupplierNo = Utilities.GetMaxLength(typeof(Supplier), nameof(Supplier.supplier_no)) ?? 50;
            var supplierNoTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_no) &&
                            x.supplier_no.Length > maxLengthOfISupplierNo).ToList();
            if (supplierNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Supplier.supplier_no), maxLengthOfISupplierNo, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            // - Status Flag E/D
            #region StatusFlag
            var invalidStatusFlag = request.ExcelData.Where(x =>
                   x.status_flag != StatusFlag.Enabled &&
                   x.status_flag != StatusFlag.Disabled).ToList();

            if (invalidStatusFlag.Count > 0)
            {
                report.AddRange(invalidStatusFlag.Select(x => new ImportReportDto
                {
                    Identifier = x.supplier_no,
                    Message = string.Format(ErrorMessages.InvalidField, nameof(x.status_flag), x.index)
                }));

                invalidRows.AddRange(invalidStatusFlag.Select(x => x.index).ToArray());
            }
            #endregion

            var lovData = request.Lovs;

            // - PO Sending Method in LOV POSendMethods
            #region PoSendingMethod
            var poSendMethods = lovData.Where(x => x.lov_type == LOVs.Type.PoSendMethod).Select(x => x.lov_value)
                    .Distinct().ToHashSet();

            if (poSendMethods.Count > 0)
            {
                var invalidPoSendingMethods = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.po_sending_method))
                    .Select(x => x.po_sending_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.PoSendMethod).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                foreach (var invalidItem in invalidPoSendingMethods)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.po_sending_method == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.po_sending_method), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            // - Payment Term in LOV PaymentTerms
            #region PaymentTerm
            var paymentTerms = lovData.Where(x => x.lov_type == LOVs.Type.PaymentTerm).Select(x => x.lov_value)
                    .Distinct().ToHashSet();

            if (paymentTerms.Count > 0)
            {
                var invalidPaymentTerms = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.payment_term))
                    .Select(x => x.payment_term).Except(lovData.Where(x => x.lov_type == LOVs.Type.PaymentTerm)
                        .Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                foreach (var invalidItem in invalidPaymentTerms)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.payment_term == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.payment_term), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            // - Cost Rule in LOV CostRules
            #region CostRule
            var costRules = lovData.Where(x => x.lov_type == LOVs.Type.CostRule).Select(x => x.lov_value).Distinct().ToHashSet();
            if (costRules.Count > 0)
            {
                var invalidCostRules = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.cost_rule))
                    .Select(x => x.cost_rule).Except(lovData.Where(x => x.lov_type == LOVs.Type.CostRule).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                foreach (var invalidItem in invalidCostRules)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.cost_rule == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.cost_rule), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            // - Inco Term in LOV IncoTerms
            #region IncoTerm
            var incoTerms = lovData.Where(x => x.lov_type == LOVs.Type.IncoTerm).Select(x => x.lov_value).Distinct().ToHashSet();
            if (incoTerms.Count > 0)
            {
                var invalidIncoTerms = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.inco_term))
                    .Select(x => x.inco_term).Except(lovData.Where(x => x.lov_type == LOVs.Type.IncoTerm).Select(x => x.lov_value))
                    .Distinct().ToHashSet();

                foreach (var invalidItem in invalidIncoTerms)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.inco_term == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.inco_term), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            // - Default Freight Method in LOV FreightMethods
            #region FreightMethod
            var freightMethods = lovData.Where(x => x.lov_type == LOVs.Type.FreightMethod).Select(x => x.lov_value).Distinct().ToHashSet();

            if (freightMethods.Count > 0)
            {
                var invalidFreightMethods = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_freight_method))
                    .Select(x => x.default_freight_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.FreightMethod).Select(x => x.lov_value))
                    .Distinct().ToArray();

                foreach (var invalidItem in invalidFreightMethods)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.default_freight_method == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.default_freight_method), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            #region PaymentMethod
            var paymentMethods = lovData.Where(x => x.lov_type == LOVs.Type.PaymentMethods).Select(x => x.lov_value).Distinct().ToHashSet();

            if (paymentMethods.Count > 0)
            {
                var invalidPaymentMethods = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.payment_method))
                    .Select(x => x.payment_method).Except(lovData.Where(x => x.lov_type == LOVs.Type.PaymentMethods).Select(x => x.lov_value))
                    .Distinct().ToArray();

                foreach (var invalidItem in invalidPaymentMethods)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.payment_method == invalidItem).ToArray();

                    report.AddRange(invalidExcelData.Select(x => new ImportReportDto
                    {
                        Identifier = x.supplier_no,
                        Message = string.Format(ErrorMessages.InvalidField, nameof(x.payment_method), x.index)
                    }).ToList());

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            // - Default Currency in Currency 
            #region Currency
            var invalidCurrencies = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_currency))
                .Select(x => x.default_currency).Except(request.Currencies.Select(x => x.currency_code)).Distinct().ToHashSet();

            foreach (var invalidItem in invalidCurrencies)
            {
                var invalidExcelData = request.ExcelData.Where(x => x.default_currency == invalidItem).ToArray();

                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierData.default_currency),
                        invalidItem, string.Join(",", invalidExcelData.Select(x => x.index).ToArray()))
                });

                invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
            }
            #endregion

            #region Country
            var invalidCountries = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_country_of_loading))
                .Select(x => x.default_country_of_loading).Distinct().Except(request.Countries.Select(x => x.country_alpha_code_two)).Distinct().ToList();

            invalidCountries.AddRange(request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_country_of_discharge))
                .Select(x => x.default_country_of_discharge).Distinct().Except(request.Countries.Select(x => x.country_alpha_code_two)).Distinct().ToList());

            foreach (var invalidItem in invalidCountries)
            {
                var invalidExcelData =
                    request.ExcelData.Where(x => x.default_country_of_loading == invalidItem).ToArray();

                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierData.default_country_of_loading),
                        invalidItem, string.Join(",", invalidExcelData.Select(x => x.index).ToArray()))
                });
            }
            #endregion

            #region Port
            var invalidPorts = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_port_of_loading))
                .Select(x => x.default_port_of_loading).Distinct().Except(request.Ports.Select(x => x.port_no)).Distinct().ToList();

            invalidPorts.AddRange(request.ExcelData.Where(x => !string.IsNullOrEmpty(x.default_port_of_discharge))
                .Select(x => x.default_port_of_discharge).Distinct().Except(request.Ports.Select(x => x.port_no)).Distinct().ToList());

            foreach (var invalidItem in invalidPorts)
            {
                var invalidExcelData = request.ExcelData.Where(x => x.default_port_of_loading == invalidItem).ToArray();

                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierData.default_port_of_loading),
                        invalidItem, string.Join(",", invalidExcelData.Select(x => x.index).ToArray()))
                });
            }
            #endregion

            #region SiteNo
            var invalidSiteNos = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.site_no))
                .Select(x => x.site_no).Distinct().Except(request.Sites.Select(x => x.site_no)).Distinct().ToArray();

            foreach (var invalidItem in invalidSiteNos)
            {
                var invalidExcelData = request.ExcelData.Where(x => x.site_no == invalidItem).ToArray();

                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierData.site_no),
                        invalidItem, string.Join(",", invalidExcelData.Select(x => x.index).ToArray()))
                });

                invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows, report);
        }

        /// <summary>
        /// Validate excel data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelSupplierItemDataAsync(ValidateSupplierItemMappingRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region SupplierNo
            // missing supplierNo
            var missingSupplierNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.supplier_no)).ToList();

            if (missingSupplierNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierItemData.supplier_no), string.Join(",", missingSupplierNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSupplierNo.Select(x => x.index).ToArray());
            }

            // invalid suppler no
            var invalidSupplierNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                .Select(x => x.supplier_no).Except(request.Suppliers.Select(y => y.supplier_no)).Distinct().ToArray();
            if (invalidSupplierNo.Length > 0)
            {
                foreach (var item in invalidSupplierNo)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.supplier_no == item).ToList();
                    report.Add(new ImportReportDto
                    {
                        Identifier = item,
                        Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierItemData.supplier_no), item, string.Join(",", invalidExcelData.Select(x => x.index)))
                    });

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            #region itemNo
            // missing itemNo
            var missingItemNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.item_no)).ToList();

            if (missingItemNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierItemData.supplier_no), string.Join(",", missingSupplierNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingItemNo.Select(x => x.index).ToArray());
            }

            // invalid itemNo
            var invalidItemNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.item_no))
                .Select(x => x.item_no).Except(request.Items.Select(y => y.item_no)).Distinct().ToArray();
            if (invalidItemNo.Length > 0)
            {
                foreach (var item in invalidItemNo)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.item_no == item).ToList();
                    report.Add(new ImportReportDto
                    {
                        Identifier = item,
                        Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierItemData.item_no), item, string.Join(",", invalidExcelData.Select(x => x.index)))
                    });

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            #region Supplier Part Number
            var maxLengthSupplierPartNo = Utilities.GetMaxLength(typeof(SupplierItemMapping), nameof(SupplierItemMapping.supplier_part_no)) ?? 100;
            var supplierPartNoTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_part_no) &&
                            x.supplier_part_no.Length > maxLengthSupplierPartNo).ToList();
            if (supplierPartNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierPartNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(SupplierItemMapping.supplier_part_no), maxLengthSupplierPartNo, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            // If Supplier ID and Item ID is the same, supplier material code is unique.
            var duplicatedGroupByKey = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_no) && !string.IsNullOrEmpty(x.item_no))
                .GroupBy(x => new { x.supplier_no, x.item_no, x.supplier_material_code })
                .Where(x => x.Count() > 1)
                .Select(x => new
                {
                    key = x.Key,
                    data = x.ToList()
                })
                .ToList();

            if (duplicatedGroupByKey.Count > 0)
            {
                foreach (var item in duplicatedGroupByKey)
                {
                    var key = $"{item.key.supplier_no}|{item.key.item_no}|{item.key.supplier_material_code ?? "null"}";
                    report.Add(new ImportReportDto
                    {
                        Identifier = key,
                        Message = string.Format(ErrorMessages.DuplicatedSupplierItemMapping, key,
                            string.Join(",", item.data.Select(x => x.index)))
                    });
                    invalidRows.AddRange(item.data.Select(x => x.index).ToArray());
                }
            }

            #region Supplier material code
            var maxLengthSupplierMaterialCode = Utilities.GetMaxLength(typeof(SupplierItemMapping), nameof(SupplierItemMapping.supplier_material_code)) ?? 50;
            var supplierMaterialCodeTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_material_code) &&
                            x.supplier_material_code.Length > maxLengthSupplierMaterialCode).ToList();
            if (supplierMaterialCodeTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierMaterialCodeTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(SupplierItemMapping.supplier_material_code), maxLengthSupplierMaterialCode, string.Join(",", item.index))
                    });
                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Supplier material description
            var maxLengthSupplierMaterialDescription = Utilities.GetMaxLength(typeof(SupplierItemMapping), nameof(SupplierItemMapping.supplier_material_description)) ?? 255;
            var supplierMaterialDescriptionTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_material_description) &&
                            x.supplier_material_description.Length > maxLengthSupplierMaterialDescription).ToList();

            if (supplierMaterialDescriptionTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierMaterialDescriptionTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(SupplierItemMapping.supplier_material_description), maxLengthSupplierMaterialCode, string.Join(",", item.index))
                    });
                    invalidRows.Add(item.index);
                }
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows.Distinct().ToList(), report);
        }

        /// <summary>
        /// Validate excel data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelSupplierSecondaryDataAsync(ValidateSupplierSecondaryRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region SupplierNo
            // missing supplier_no
            var missingSupplierNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.supplier_no)).ToList();

            if (missingSupplierNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierSecondaryData.supplier_no), string.Join(",", missingSupplierNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSupplierNo.Select(x => x.index).ToArray());
            }

            // invalid suppler no
            var invalidSupplierNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.supplier_no))
                .Select(x => x.supplier_no).Except(request.Suppliers.Select(y => y.supplier_no)).Distinct().ToArray();
            if (invalidSupplierNo.Length > 0)
            {
                foreach (var item in invalidSupplierNo)
                {
                    var invalidExcelData = request.ExcelData.Where(x => x.supplier_no == item).ToList();
                    report.Add(new ImportReportDto
                    {
                        Identifier = item,
                        Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSupplierSecondaryData.supplier_no), item, string.Join(",", invalidExcelData.Select(x => x.index)))
                    });

                    invalidRows.AddRange(invalidExcelData.Select(x => x.index).ToArray());
                }
            }
            #endregion

            #region SecondarySupplierNo
            // missing secondary_supplier_no
            var missingSecondarySupplierNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.sec_supplier_no)).ToList();

            if (missingSecondarySupplierNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierSecondaryData.sec_supplier_no), string.Join(",", missingSecondarySupplierNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSecondarySupplierNo.Select(x => x.index).ToArray());
            }

            // duplicate supplier_no
            var duplicatedSecSupplierNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.sec_supplier_no))
                .GroupBy(x => x.sec_supplier_no).Where(x => x.Count() > 1).ToList();

            if (duplicatedSecSupplierNo.Count > 0)
            {
                foreach (var duplicatedRows in duplicatedSecSupplierNo)
                {
                    report.Add(new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.DuplicateField, nameof(ImportSupplierSecondaryData.sec_supplier_no), string.Join(",", duplicatedRows.Select(x => x.index)))
                    });

                    invalidRows.AddRange(duplicatedRows.Select(x => x.index).ToArray());
                }
            }

            // secondary_supplier_no too long
            var maxLengthOfISecSupplierNo = Utilities.GetMaxLength(typeof(SecondarySupplier), nameof(SecondarySupplier.sec_supplier_no)) ?? 50;
            var supplierNoTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.supplier_no) &&
                            x.supplier_no.Length > maxLengthOfISecSupplierNo).ToList();
            if (supplierNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(ImportSupplierSecondaryData.sec_supplier_no), maxLengthOfISecSupplierNo, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region SecondarySupplierName
            // missing secondary_supplier_name
            var missingSecondarySupplierName = request.ExcelData.Where(x => string.IsNullOrEmpty(x.sec_supplier_name)).ToList();

            if (missingSecondarySupplierName.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSupplierSecondaryData.sec_supplier_name), string.Join(",", missingSecondarySupplierName.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSecondarySupplierName.Select(x => x.index).ToArray());
            }

            // secondary_supplier_name too long
            var maxLengthOfISecSupplierName = Utilities.GetMaxLength(typeof(SecondarySupplier), nameof(SecondarySupplier.sec_supplier_name)) ?? 100;
            var supplierNameTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.sec_supplier_name) &&
                            x.sec_supplier_name.Length > maxLengthOfISecSupplierName).ToList();

            if (supplierNameTooLongFromExcel.Count > 0)
            {
                foreach (var item in supplierNameTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.sec_supplier_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(ImportSupplierSecondaryData.sec_supplier_name), maxLengthOfISecSupplierName, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows.Distinct().ToList(), report);
        }

        private async Task ValidateUpdateSupplierAsync(List<Supplier> suppliers)
        {
            var invalidSupplierNos = await _supplierRepository.CheckInvalidSupplierNOs(suppliers);
            if (invalidSupplierNos.Length > 0)
            {
                throw new NotFoundException(string.Format(ErrorMessages.SupplierAlreadyExists, nameof(Supplier.supplier_no), string.Join(",", invalidSupplierNos)));
            }

            await ValidateSupplierBasedLovDataAsync(suppliers);
        }
        #endregion
    }
}
