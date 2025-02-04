using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Countries.DTOs.Response;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Application.Masters.Sites.DTOs.Request;
using SERP.Application.Masters.Sites.DTOs.Response;
using SERP.Application.Masters.Sites.Interfaces;
using SERP.Domain.Common.Enums;
using SERP.Domain.Masters.Countries;
using SERP.Domain.Masters.Sites;
using System.Data;
using System.Text;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Sites.Services
{
    internal class SiteService : ISiteService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SiteService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISiteRepository _siteRepository;

        public SiteService(
            ICountryRepository countryRepository,
            IMapper mapper,
            ILogger<SiteService> logger,
            IUnitOfWork unitOfWork,
            ISiteRepository siteRepository)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _siteRepository = siteRepository;
        }

        public async Task<List<SiteResponseDto>> GetAllLimitedAsync()
        {
            return _mapper.Map<List<SiteResponseDto>>(await _siteRepository.GetAllLimited());
        }

        public async Task<object> ImportSiteAsync(string userId, IFormFile file)
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
                var excelData = await ReadExcelSiteDataAsync(inputFilePath);

                var countryCodes = excelData.Where(x => !string.IsNullOrEmpty(x.country))
                    .Select(x => x.country!).Distinct().ToHashSet();
                var countries = await GetCountryList(countryCodes);

                var validationResult = ValidateExcelSiteDataAsync(new ValidateImportSiteRequest()
                {
                    ExcelData = excelData,
                    Countries = countries
                });

                var excelDataValid = excelData.Where(x => !validationResult.Item1.Contains(x.index)).ToList();

                var siteNos = excelDataValid.Select(x => x.site_no).ToHashSet();
                var dicSties = await _siteRepository.GetDictionaryBySiteNoAsync(siteNos);

                var siteToInsert = new List<Site>();
                var siteToUpdate = new List<Site>();
                foreach (var importData in excelDataValid)
                {
                    var countryId = countries.FirstOrDefault(x => x.country_alpha_code_two == importData.country)?.id;

                    if (dicSties.TryGetValue(importData.site_no!, out var site))
                    {
                        // update
                        _mapper.Map(importData, site);
                        site.country_id = countryId;
                        site.last_modified_by = userId;
                        site.last_modified_on = currentTime;
                        siteToUpdate.Add(site);
                    }
                    else
                    {
                        // create
                        site = _mapper.Map<Site>(importData);
                        site.country_id = countryId;
                        site.created_by = userId;
                        site.created_on = currentTime;
                        siteToInsert.Add(site);
                    }
                }

                if (siteToUpdate.Count > 0)
                {
                    await _siteRepository.UpdateRangeAsync(siteToUpdate);
                }

                if (siteToInsert.Count > 0)
                {
                    await _siteRepository.CreateRangeAsync(siteToInsert);
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

        public async Task<int> CreateSiteAsync(string userId, CreateSiteRequestDto request)
        {
            var validateRequest = _mapper.Map<ValidateSiteRequest>(request);
            await ValidateSite(validateRequest);

            var site = _mapper.Map<Site>(request);
            site.created_by = userId;
            site.created_on = DateTime.Now;

            await _siteRepository.CreateAsync(site);
            await _unitOfWork.SaveChangesAsync();
            return site.id;
        }

        public async Task UpdateSiteAsync(string userId, UpdateSiteRequestDto request)
        {
            var site = await _siteRepository.GetByIdAsync(x => x.id == request.id);
            if (site is null)
            {
                throw new NotFoundException(ErrorCodes.SiteNotFound, string.Format(ErrorMessages.SiteNotFound, nameof(request.id), request.id));
            }

            var validateRequest = _mapper.Map<ValidateSiteRequest>(request);
            await ValidateSite(validateRequest);

            _mapper.Map(request, site);
            site.last_modified_by = userId;
            site.last_modified_on = DateTime.Now;

            await _siteRepository.UpdateAsync(site);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSiteAsync(int id)
        {
            var site = await _siteRepository.GetByIdAsync(x => x.id == id);
            if (site is null)
            {
                throw new NotFoundException(ErrorCodes.SiteNotFound, string.Format(ErrorMessages.SiteNotFound, nameof(id), id));
            }

            await _siteRepository.DeleteAsync(site);
            await _unitOfWork.SaveChangesAsync();
        }

        #region Private methods
        /// <summary>
        /// Read excel
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <returns></returns>

        private async Task<List<ImportSiteData>> ReadExcelSiteDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportSiteData>();
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
                var rowData = new ImportSiteData
                {
                    index = rowIndex++,

                    site_no = item[SiteExcelColumns.SiteNo] != DBNull.Value
                        ? item[SiteExcelColumns.SiteNo].ToString()
                        : null,

                    site_name = item[SiteExcelColumns.SiteName] != DBNull.Value
                        ? item[SiteExcelColumns.SiteName].ToString()
                        : null,

                    address_line_1 = item[SiteExcelColumns.AddressLine1] != DBNull.Value
                        ? item[SiteExcelColumns.AddressLine1].ToString()
                        : null,

                    address_line_2 = item[SiteExcelColumns.AddressLine2] != DBNull.Value
                        ? item[SiteExcelColumns.AddressLine2].ToString()
                        : null,

                    address_line_3 = item[SiteExcelColumns.AddressLine3] != DBNull.Value
                        ? item[SiteExcelColumns.AddressLine3].ToString()
                        : null,

                    address_line_4 = item[SiteExcelColumns.AddressLine4] != DBNull.Value
                        ? item[SiteExcelColumns.AddressLine4].ToString()
                        : null,

                    city = item[SiteExcelColumns.City] != DBNull.Value
                        ? item[SiteExcelColumns.City].ToString()
                        : null,

                    state_province = item[SiteExcelColumns.StateProvince] != DBNull.Value
                        ? item[SiteExcelColumns.StateProvince].ToString()
                        : null,

                    postal_code = item[SiteExcelColumns.PostalCode] != DBNull.Value
                        ? item[SiteExcelColumns.PostalCode].ToString()
                        : null,

                    country = item[SiteExcelColumns.Country] != DBNull.Value
                        ? item[SiteExcelColumns.Country].ToString()
                        : null,

                    county = item[SiteExcelColumns.County] != DBNull.Value
                        ? item[SiteExcelColumns.County].ToString()
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

        private async Task<List<Country>> GetCountryList(HashSet<string>? countryCodes)
        {
            if (countryCodes is null)
            {
                return [];
            }

            var countries = await _countryRepository.GetCountryCodeListAsync(countryCodes);

            return countries;
        }

        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelSiteDataAsync(ValidateImportSiteRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region SiteNo
            // missing site no
            var missingSiteNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.site_no)).ToList();
            if (missingSiteNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSiteData.site_no), string.Join(",", missingSiteNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSiteNo.Select(x => x.index).ToList());
            }


            // duplicate site no
            var duplicateSiteNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.site_no))
                .GroupBy(x => x.site_no).Where(x => x.Count() > 1).ToList();

            if (duplicateSiteNo.Count > 0)
            {
                foreach (var item in duplicateSiteNo)
                {
                    report.Add(new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.DuplicateField, nameof(ImportSiteData.site_no),
                            string.Join(",", item.Select(x => x.index)))
                    });

                    invalidRows.AddRange(item.Select(x => x.index).ToList());
                }
            }

            // site no too long
            // supplier no too long
            var maxLengthOfISiteNo = Utilities.GetMaxLength(typeof(Site), nameof(Site.site_no)) ?? 50;
            var siteNoTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.site_no) &&
                            x.site_no.Length > maxLengthOfISiteNo).ToList();
            if (siteNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in siteNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.site_no), maxLengthOfISiteNo, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Site name
            // missing site name
            var missingSupplierName = request.ExcelData.Where(x => string.IsNullOrEmpty(x.site_name)).ToList();
            if (missingSupplierName.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportSiteData.site_name),
                        string.Join(",", missingSupplierName.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSupplierName.Select(x => x.index).ToList());
            }

            // site name too long
            var maxLengthOfISupplierName = Utilities.GetMaxLength(typeof(Site), nameof(Site.site_name)) ?? 255;
            var siteNameTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.site_name) && x.site_name.Length > maxLengthOfISupplierName).ToList();

            if (siteNameTooLongFromExcel.Count > 0)
            {
                foreach (var item in siteNameTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.site_name), maxLengthOfISupplierName, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Address line 1
            // address line 1 too long
            var maxLengthOfIAddressLine1 = Utilities.GetMaxLength(typeof(Site), nameof(Site.address_line_1)) ?? 255;
            var addressLine1TooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.address_line_1) && x.address_line_1.Length > maxLengthOfIAddressLine1).ToList();

            if (addressLine1TooLongFromExcel.Count > 0)
            {
                foreach (var item in addressLine1TooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.address_line_1), maxLengthOfIAddressLine1, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Address line 2
            var maxLengthOfIAddressLine2 = Utilities.GetMaxLength(typeof(Site), nameof(Site.address_line_2)) ?? 255;
            var addressLine2TooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.address_line_2) && x.address_line_2.Length > maxLengthOfIAddressLine2).ToList();

            if (addressLine2TooLongFromExcel.Count > 0)
            {
                foreach (var item in addressLine2TooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.address_line_2), maxLengthOfIAddressLine2, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Address line 3
            var maxLengthOfIAddressLine3 = Utilities.GetMaxLength(typeof(Site), nameof(Site.address_line_3)) ?? 255;
            var addressLine3TooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.address_line_3) && x.address_line_3.Length > maxLengthOfIAddressLine3).ToList();

            if (addressLine3TooLongFromExcel.Count > 0)
            {
                foreach (var item in addressLine3TooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.address_line_3),
                            maxLengthOfIAddressLine3, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Address line 4
            var maxLengthOfIAddressLine4 = Utilities.GetMaxLength(typeof(Site), nameof(Site.address_line_4)) ?? 255;
            var addressLine4TooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.address_line_4) && x.address_line_4.Length > maxLengthOfIAddressLine4).ToList();

            if (addressLine4TooLongFromExcel.Count > 0)
            {
                foreach (var item in addressLine4TooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.address_line_4),
                            maxLengthOfIAddressLine4, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region City
            var maxLengthOfICity = Utilities.GetMaxLength(typeof(Site), nameof(Site.city)) ?? 50;
            var cityTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.city) && x.city.Length > maxLengthOfICity).ToList();

            if (cityTooLongFromExcel.Count > 0)
            {
                foreach (var item in cityTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.city),
                            maxLengthOfICity, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region state/province
            var maxLengthOfIStateProvince = Utilities.GetMaxLength(typeof(Site), nameof(Site.state_province)) ?? 50;
            var stateProvinceTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.state_province) && x.state_province.Length > maxLengthOfIStateProvince).ToList();

            if (stateProvinceTooLongFromExcel.Count > 0)
            {
                foreach (var item in stateProvinceTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.state_province),
                            maxLengthOfIStateProvince, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region postal code
            var maxLengthOfIPostalCode = Utilities.GetMaxLength(typeof(Site), nameof(Site.postal_code)) ?? 20;
            var postalCodeTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.postal_code) && x.postal_code.Length > maxLengthOfIPostalCode).ToList();

            if (postalCodeTooLongFromExcel.Count > 0)
            {
                foreach (var item in postalCodeTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.postal_code),
                            maxLengthOfIPostalCode, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region County
            var maxLengthOfICounty = Utilities.GetMaxLength(typeof(Site), nameof(Site.county)) ?? 50;
            var countyTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.county) && x.county.Length > maxLengthOfICounty).ToList();

            if (countyTooLongFromExcel.Count > 0)
            {
                foreach (var item in countyTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.site_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Site.county),
                            maxLengthOfICounty, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Country
            var invalidCountryCodes = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.country))
                .Select(x => x.country).Except(request.Countries.Select(y => y.country_alpha_code_two)).Distinct().ToList();

            if (invalidCountryCodes.Count > 0)
            {
                foreach (var item in invalidCountryCodes)
                {
                    var invalidItem = request.ExcelData.Where(x => x.country == item).ToList();
                    report.Add( new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportSiteData.country), item, string.Join(",", invalidItem.Select(x => x.index)))
                    });

                    invalidRows.AddRange(invalidItem.Select(x => x.index));
                }
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows, report);
        }

        private async Task ValidateSite(ValidateSiteRequest request)
        {
            if (request.country_id.HasValue)
            {
                var isCountryExisted = await _countryRepository.CountryExistsAsync(request.country_id.Value);
                if (!isCountryExisted)
                {
                    throw new NotFoundException(ErrorCodes.CountryNotFound, string.Format(ErrorMessages.CountryNotFound, nameof(request.country_id), request.country_id));
                }
            }

            if (request.mode == SERPEnum.Mode.Insert)
            {
                var isExistedSiteNo = await _siteRepository.SiteExistsAsync(request.site_no);
                if (isExistedSiteNo)
                {
                    throw new NotFoundException(ErrorCodes.SiteAlreadyExists, string.Format(ErrorMessages.SiteAlreadyExists, nameof(request.site_no), request.site_no));
                }
            }
        }
        #endregion
    }
}
