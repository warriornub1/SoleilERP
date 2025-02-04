using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Items.DTOs;
using SERP.Application.Masters.Items.DTOs.Response;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Domain.Masters.Items;
using System.Data;
using System.Text;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Items.Services
{
    public class ItemService : IItemService
    {
        private readonly ILogger<ItemService> _logger;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(
            ILogger<ItemService> logger,
            IItemRepository itemRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ItemLimitedDto>> GetAllLimited(bool onlyEnabled)
        {
            return _mapper.Map<List<ItemLimitedDto>>(await _itemRepository.GetAllLimited(onlyEnabled));
        }

        public async Task<ItemDto> GetById(int id)
        {
            return _mapper.Map<ItemDto>(await _itemRepository.GetById(id));
        }

        public async Task<object> ImportItemAsync(string userId, IFormFile file)
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
                var excelData = await ReadExcelItemDataAsync(inputFilePath);

                var validationResult = await ValidateExcelItemDataAsync(excelData);

                var noInvalid = validationResult.Select(x => x.Identifier).Distinct().ToHashSet();
                var excelDataValid = excelData.Where(x => !noInvalid.Contains(x.item_no)).ToList();

                var dicItems = await _itemRepository.GetDictionaryByItemNo(excelDataValid.Select(x => x.item_no).ToList());

                var itemToInsert = new List<Item>();
                var itemToUpdate = new List<Item>();
                foreach (var importData in excelDataValid)
                {
                    if (dicItems.TryGetValue(importData.item_no!, out var item))
                    {
                        // update
                        _mapper.Map(importData, item);
                        item.last_modified_by = userId;
                        item.last_modified_on = currentTime;
                        itemToUpdate.Add(item);
                    }
                    else
                    {
                        // create
                        item = _mapper.Map<Item>(importData);
                        item.created_by = userId;
                        item.created_on = currentTime;
                        itemToInsert.Add(item);
                    }
                }

                if (itemToUpdate.Count > 0)
                {
                    await _itemRepository.UpdateRangeAsync(itemToUpdate);
                }

                if (itemToInsert.Count > 0)
                {
                    await _itemRepository.CreateRangeAsync(itemToInsert);
                }

                await _unitOfWork.SaveChangesAsync();
                return validationResult;
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

        #region Private Methods
        private async Task<List<ImportReportDto>> ValidateExcelItemDataAsync(List<ImportItemData> excelData)
        {
            var report = new List<ImportReportDto>();

            #region ItemNo
            // missing item no
            var missingItemNo = excelData.Where(x => string.IsNullOrEmpty(x.item_no)).ToList();

            if (missingItemNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportItemData.item_no), string.Join(",", missingItemNo.Select(x => x.index)))
                });
            }

            // duplicate item no
            var duplicatedItemNo = excelData.Where(x => !string.IsNullOrEmpty(x.item_no))
                .GroupBy(x => x.item_no)
                .Where(x => x.Count() > 1).ToList();

            if (duplicatedItemNo.Count > 0)
            {
                foreach (var invalidRows in duplicatedItemNo)
                {
                    report.Add(new ImportReportDto()
                    {
                        Message = string.Format(ErrorMessages.DuplicateField, nameof(ImportItemData.item_no), string.Join(",", invalidRows.Select(x => x.index)))
                    });
                }
            }

            // item no too long
            var maxLengthOfItemNo = Utilities.GetMaxLength(typeof(Item), nameof(Item.item_no)) ?? 50;
            var itemNoTooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.brand) &&
                            x.brand.Length > maxLengthOfItemNo).ToList();
            if (itemNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in itemNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.item_no), maxLengthOfItemNo, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region Description 1
            // missing description 1
            var missingDescription1 = excelData.Where(x => string.IsNullOrEmpty(x.brand)).ToList();

            if (missingDescription1.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportItemData.description_1), string.Join(",", missingDescription1.Select(x => x.index)))
                });
            }

            // description 1 too long
            var maxLengthOfDescription1 = Utilities.GetMaxLength(typeof(Item), nameof(Item.description_1)) ?? 255;
            var description1TooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.description_1) &&
                            x.description_1.Length > maxLengthOfDescription1).ToList();

            if (description1TooLongFromExcel.Count > 0)
            {
                foreach (var item in description1TooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.description_1), maxLengthOfDescription1, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region Description 2
            // description 1 too long
            var maxLengthOfDescription2 = Utilities.GetMaxLength(typeof(Item), nameof(Item.description_1)) ?? 255;
            var description2TooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.description_2) &&
                            x.description_2.Length > maxLengthOfDescription2).ToList();

            if (description2TooLongFromExcel.Count > 0)
            {
                foreach (var item in description2TooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.description_2), maxLengthOfDescription2, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region Brand
            var maxLengthOfBrand = Utilities.GetMaxLength(typeof(Item), nameof(Item.brand)) ?? 50;
            var brandTooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.brand) &&
                            x.brand.Length > maxLengthOfBrand).ToList();
            if (brandTooLongFromExcel.Count > 0)
            {
                foreach (var item in brandTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.brand), maxLengthOfBrand, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region primary_uom
            var missingPrimaryUom = excelData.Where(x => string.IsNullOrEmpty(x.primary_uom)).ToList();

            if (missingPrimaryUom.Count > 0)
            {
                report.Add(new ImportReportDto()
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportItemData.primary_uom),
                        string.Join(",", missingPrimaryUom.Select(x => x.index)))
                });
            }

            var maxLengthOfPrimaryUom = Utilities.GetMaxLength(typeof(Item), nameof(Item.primary_uom)) ?? 5;
            var primaryUomTooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.primary_uom) &&
                            x.primary_uom.Length > maxLengthOfPrimaryUom).ToList();
            if (primaryUomTooLongFromExcel.Count > 0)
            {
                foreach (var item in primaryUomTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.primary_uom), maxLengthOfPrimaryUom, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region SecondaryUom
            // missing SecondaryUom
            var missingSecondaryUom = excelData.Where(x => string.IsNullOrEmpty(x.secondary_uom)).ToList();

            if (missingSecondaryUom.Count > 0)
            {
                report.Add(new ImportReportDto()
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportItemData.secondary_uom),
                        string.Join(",", missingItemNo.Select(x => x.index)))
                });
            }

            var maxLengthOfSecondaryUom = Utilities.GetMaxLength(typeof(Item), nameof(Item.secondary_uom)) ?? 5;
            var secondaryUomTooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.secondary_uom) &&
                            x.secondary_uom.Length > maxLengthOfSecondaryUom).ToList();

            if (secondaryUomTooLongFromExcel.Count > 0)
            {
                foreach (var item in secondaryUomTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.secondary_uom), maxLengthOfSecondaryUom, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region PurchaseUom
            // missing PurchaseUom
            var missingPurchaseUom = excelData.Where(x => string.IsNullOrEmpty(x.purchasing_uom)).ToList();
            if (missingPurchaseUom.Count > 0)
            {
                report.Add(new ImportReportDto()
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportItemData.purchasing_uom),
                        string.Join(",", missingPurchaseUom.Select(x => x.index)))
                });
            }

            var maxLengthOfPurchaseUom = Utilities.GetMaxLength(typeof(Item), nameof(Item.purchasing_uom)) ?? 5;
            var purchaseUomTooLongFromExcel = excelData
                .Where(x => !string.IsNullOrEmpty(x.purchasing_uom) &&
                            x.purchasing_uom.Length > maxLengthOfPurchaseUom).ToList();

            if (purchaseUomTooLongFromExcel.Count > 0)
            {
                foreach (var item in purchaseUomTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.item_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Item.purchasing_uom), maxLengthOfPurchaseUom, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region StatusFlag
            var invalidStatusFlag = excelData.Where(x =>
                x.status_flag != ApplicationConstant.StatusFlag.Enabled &&
                x.status_flag != ApplicationConstant.StatusFlag.Disabled).ToList();

            if (invalidStatusFlag.Count > 0)
            {
                report.AddRange(invalidStatusFlag.Select(x => new ImportReportDto()
                {
                    Identifier = x.item_no,
                    Message = string.Format(ErrorMessages.InvalidField, nameof(x.status_flag), x.index)
                }));
            }
            #endregion

            return report;
        }

        private async Task<List<ImportItemData>> ReadExcelItemDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index", "purchase_min_order_qty", "purchase_multiple_order_qty" };
            var result = new List<ImportItemData>();
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
                var rowData = new ImportItemData
                {
                    index = rowIndex++,

                    item_no = item[ItemExcelColumns.ItemNo] != DBNull.Value
                        ? item[ItemExcelColumns.ItemNo].ToString()
                        : null,

                    description_1 = item[ItemExcelColumns.Description1] != DBNull.Value
                        ? item[ItemExcelColumns.Description1].ToString()
                        : null,

                    description_2 = item[ItemExcelColumns.Description2] != DBNull.Value
                        ? item[ItemExcelColumns.Description2].ToString()
                        : null,

                    brand = item[ItemExcelColumns.Brand] != DBNull.Value
                        ? item[ItemExcelColumns.Brand].ToString()
                        : null,

                    primary_uom = item[ItemExcelColumns.PrimaryUOM] != DBNull.Value
                        ? item[ItemExcelColumns.PrimaryUOM].ToString()
                        : null,

                    secondary_uom = item[ItemExcelColumns.SecondaryUOM] != DBNull.Value
                        ? item[ItemExcelColumns.SecondaryUOM].ToString()
                        : null,

                    purchasing_uom = item[ItemExcelColumns.PurchasingUOM] != DBNull.Value
                        ? item[ItemExcelColumns.PurchasingUOM].ToString()
                        : null,

                    status_flag = item[ItemExcelColumns.StatusFlag] != DBNull.Value
                        ? item[ItemExcelColumns.StatusFlag].ToString()
                        : null,

                    purchase_min_order_qty = item[ItemExcelColumns.PurchaseMinOrderQty] != DBNull.Value
                        ? int.TryParse(item[ItemExcelColumns.PurchaseMinOrderQty].ToString(), out var purchaseMinOrderQty)
                            ? purchaseMinOrderQty
                            : 0
                        : 0,

                    purchase_multiple_order_qty = item[ItemExcelColumns.PurchaseMultipleOrderQty] != DBNull.Value
                        ? int.TryParse(item[ItemExcelColumns.PurchaseMultipleOrderQty].ToString(), out var purchaseMultipleOrderQty)
                            ? purchaseMultipleOrderQty
                            : 0
                        : 0
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
        #endregion
    }
}
