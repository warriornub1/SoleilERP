using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Countries.DTOs.Request;
using SERP.Application.Masters.Countries.DTOs.Response;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Domain.Masters.Countries;
using System.Data;
using System.Text;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Countries.Services
{
    internal class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(ICountryRepository countryRepository,
            IMapper mapper,
            ILogger<CountryService> logger,
            IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CountryResponseDto>> GetAllLimitedAsync()
        {
            return _mapper.Map<List<CountryResponseDto>>(await _countryRepository.GetAllLimited());
        }

        public async Task<object> ImportCountryAsync(string userId, IFormFile file)
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
                var excelData = await ReadExcelCountryDataAsync(inputFilePath);

                var validationResult = ValidateExcelCountryDataAsync(new ValidateCountryRequest()
                {
                    ExcelData = excelData
                });

                var noInvalid = validationResult.Item1.Distinct().ToList();
                var excelDataValid = excelData.Where(x => !noInvalid.Contains(x.index)).ToList();

                var validCountryCode2Digits = excelDataValid.Select(x => x.country_alpha_code_two!).Distinct().ToList();
                var dicCountries = await _countryRepository.GetByCountryCode2DigitsAsync(validCountryCode2Digits);

                var countryToInsert = new List<Country>();
                var countryToUpdate = new List<Country>();
                foreach (var importData in excelDataValid)
                {
                    if (dicCountries.TryGetValue(importData.country_alpha_code_two!, out var country))
                    {
                        // update
                        _mapper.Map(importData, country);
                        country.last_modified_by = userId;
                        country.last_modified_on = currentTime;

                        countryToUpdate.Add(country);
                    }
                    else
                    {
                        // create
                        country = _mapper.Map<Country>(importData);
                        country.created_by = userId;
                        country.created_on = currentTime;

                        countryToInsert.Add(country);
                    }
                }

                if (countryToUpdate.Count > 0)
                {
                    await _countryRepository.UpdateRangeAsync(countryToUpdate);
                }

                if (countryToInsert.Count > 0)
                {
                    await _countryRepository.CreateRangeAsync(countryToInsert);
                }

                await _unitOfWork.SaveChangesAsync();
                return validationResult.Item2;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            finally
            {
                Directory.Delete(localFilePath, true);
            }
        }

        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelCountryDataAsync(ValidateCountryRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region country name
            // missing Port no
            var missingCountryName = request.ExcelData.Where(x => string.IsNullOrEmpty(x.country_name)).ToList();
            if (missingCountryName.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportCountryData.country_name), string.Join(",", missingCountryName.Select(x => x.index)))
                });

                invalidRows.AddRange(missingCountryName.Select(x => x.index).ToList());
            }

            // country name too long
            var maxLengthOfICountryName = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_name)) ?? 50;
            var countryNameTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_name) &&
                            x.country_name.Length > maxLengthOfICountryName).ToList();
            if (countryNameTooLongFromExcel.Count > 0)
            {
                foreach (var item in countryNameTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.country_name), maxLengthOfICountryName, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region country long name
            // country name too long
            var maxLengthOfICountryLongName = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_long_name)) ?? 255;
            var countryLongNameTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_long_name) &&
                            x.country_long_name.Length > maxLengthOfICountryLongName).ToList();
            if (countryLongNameTooLongFromExcel.Count > 0)
            {
                foreach (var item in countryLongNameTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.country_long_name), maxLengthOfICountryLongName, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region country code 2 digts
            // missing country code 2 digits
            var missingCountryCode2Digits = request.ExcelData.Where(x => string.IsNullOrEmpty(x.country_alpha_code_two)).ToList();
            if (missingCountryCode2Digits.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportCountryData.country_alpha_code_two),
                        string.Join(",", missingCountryCode2Digits.Select(x => x.index)))
                });

                invalidRows.AddRange(missingCountryCode2Digits.Select(x => x.index).ToList());
            }

            // Port name too long
            var maxLengthOfICountryCode2Digits = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_alpha_code_two)) ?? 2;
            var countryCode2DigitsTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_alpha_code_two) && x.country_alpha_code_two.Length > maxLengthOfICountryCode2Digits).ToList();

            if (countryCode2DigitsTooLongFromExcel.Count > 0)
            {
                foreach (var item in countryCode2DigitsTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.country_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.country_alpha_code_two), maxLengthOfICountryCode2Digits, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region country code 3 digts
            // missing country code 3 digits
            var missingCountryCode3Digits = request.ExcelData.Where(x => string.IsNullOrEmpty(x.country_alpha_code_three)).ToList();
            if (missingCountryCode3Digits.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportCountryData.country_alpha_code_three),
                        string.Join(",", missingCountryCode3Digits.Select(x => x.index)))
                });

                invalidRows.AddRange(missingCountryCode3Digits.Select(x => x.index).ToList());
            }

            // Port name too long
            var maxLengthOfICountryCode3Digits = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_alpha_code_three)) ?? 3;
            var countryCode3DigitsTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_alpha_code_three) && x.country_alpha_code_three.Length > maxLengthOfICountryCode3Digits).ToList();

            if (countryCode3DigitsTooLongFromExcel.Count > 0)
            {
                foreach (var item in countryCode3DigitsTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.country_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.country_alpha_code_three), maxLengthOfICountryCode3Digits, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Country IDD
            var missingCountryIdd = request.ExcelData.Where(x => string.IsNullOrEmpty(x.country_idd)).ToList();
            if (missingCountryIdd.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportCountryData.country_idd),
                            string.Join(",", missingCountryIdd.Select(x => x.index)))
                });
            }

            // Country IDD too long
            var maxLengthOfICountryIdd = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_idd)) ?? 3;
            var countryIddTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_idd) && x.country_idd.Length > maxLengthOfICountryIdd).ToList();
            if (countryIddTooLongFromExcel.Count > 0)
            {
                foreach (var item in countryIddTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.country_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.country_idd), maxLengthOfICountryIdd, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            #region Continent
            var missingContinent = request.ExcelData.Where(x => string.IsNullOrEmpty(x.continent)).ToList();
            if (missingContinent.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportCountryData.continent),
                                        string.Join(",", missingContinent.Select(x => x.index)))
                });
            }

            // Continent too long
            var maxLengthOfIContinent = Utilities.GetMaxLength(typeof(Country), nameof(Country.continent)) ?? 25;
            var continentTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.continent) && x.continent.Length > maxLengthOfIContinent).ToList();
            if (continentTooLongFromExcel.Count > 0)
            {
                foreach (var item in continentTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.country_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Country.continent), maxLengthOfIContinent, string.Join(",", item.index))
                    });
                }
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows.Distinct().ToList(), report);
        }

        #region Private medthods
        private async Task<List<ImportCountryData>> ReadExcelCountryDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportCountryData>();
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
                var rowData = new ImportCountryData
                {
                    index = rowIndex++,

                    country_name = item[CountryExcelColumns.CountryName] != DBNull.Value
                        ? item[CountryExcelColumns.CountryName].ToString()
                        : null,
                    country_long_name = item[CountryExcelColumns.CountryLongName] != DBNull.Value
                        ? item[CountryExcelColumns.CountryLongName].ToString()
                        : null,
                    country_alpha_code_two = item[CountryExcelColumns.CountryCode2Digits] != DBNull.Value
                        ? item[CountryExcelColumns.CountryCode2Digits].ToString()
                        : null,
                    country_alpha_code_three = item[CountryExcelColumns.CountryCode3Digits] != DBNull.Value
                        ? item[CountryExcelColumns.CountryCode3Digits].ToString()
                        : null,
                    country_idd = item[CountryExcelColumns.CountryIDD] != DBNull.Value
                        ? item[CountryExcelColumns.CountryIDD].ToString()
                        : null,
                    continent = item[CountryExcelColumns.Continent] != DBNull.Value
                        ? item[CountryExcelColumns.Continent].ToString()
                        : null,
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
