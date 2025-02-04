using AutoMapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Application.Masters.Ports.DTOs.Request;
using SERP.Application.Masters.Ports.DTOs.Response;
using SERP.Application.Masters.Ports.Interfaces;
using SERP.Domain.Masters.Countries;
using SERP.Domain.Masters.Ports;
using System.Data;
using System.Text;
using static SERP.Domain.Common.Constants.DomainConstant;

namespace SERP.Application.Masters.Ports.Services
{
    internal class PortService : IPortService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IPortRepository _portRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PortService> _logger;

        public PortService(
            ICountryRepository countryRepository,
            IPortRepository portRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<PortService> logger)
        {
            _countryRepository = countryRepository;
            _portRepository = portRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<PortResponseDto>> GetByCountryCode(GetPortRequestDto request)
        {
            var ports = await _portRepository.GetByCountryCode(request.CountryCode, request.CountryId);
            if (ports.Count == 0)
            {
                throw new NotFoundException(ErrorCodes.PortNotFound, string.Format(ErrorMessages.PortNotFound, nameof(Port.country_id), request.CountryId));
            }

            return _mapper.Map<List<PortResponseDto>>(ports);
        }

        public async Task<object> ImportPortAsync(string userId, IFormFile file)
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
                var excelData = await ReadExcelPortDataAsync(inputFilePath);

                var countryCodes = excelData.Where(x => !string.IsNullOrEmpty(x.country_code))
                    .Select(x => x.country_code!).Distinct().ToHashSet();
                var countries = await GetCountryList(countryCodes);

                var validationResult = ValidateExcelPortDataAsync(new ValidatePortRequest()
                {
                    ExcelData = excelData,
                    Countries = countries
                });

                var noInvalid = validationResult.Item1.Distinct().ToList();
                var excelDataValid = excelData.Where(x => !noInvalid.Contains(x.index)).ToList();

                var dicPorts = await _portRepository.GetDictionaryByPortNoAsync(excelDataValid.Select(x => x.port_no!).ToList());

                var portToInsert = new List<Port>();
                var portToUpdate = new List<Port>();
                foreach (var importData in excelDataValid)
                {
                    var countryId = countries.FirstOrDefault(x => x.country_alpha_code_two == importData.country_code)?.id;

                    if (dicPorts.TryGetValue(importData.port_no!, out var port))
                    {
                        // update
                        _mapper.Map(importData, port);
                        port.country_id = countryId;
                        port.last_modified_by = userId;
                        port.last_modified_on = currentTime;

                        portToUpdate.Add(port);
                    }
                    else
                    {
                        // create
                        port = _mapper.Map<Port>(importData);
                        port.country_id = countryId;
                        port.created_by = userId;
                        port.created_on = currentTime;

                        portToInsert.Add(port);
                    }
                }

                if (portToUpdate.Count > 0)
                {
                    await _portRepository.UpdateRangeAsync(portToUpdate);
                }

                if (portToInsert.Count > 0)
                {
                    await _portRepository.CreateRangeAsync(portToInsert);
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

        private Tuple<List<int>, List<ImportReportDto>> ValidateExcelPortDataAsync(ValidatePortRequest request)
        {
            var report = new List<ImportReportDto>();
            var invalidRows = new List<int>();

            #region PortNo
            // missing Port no
            var missingPortNo = request.ExcelData.Where(x => string.IsNullOrEmpty(x.port_no)).ToList();
            if (missingPortNo.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportPortData.port_no), string.Join(",", missingPortNo.Select(x => x.index)))
                });

                invalidRows.AddRange(missingPortNo.Select(x => x.index).ToList());
            }

            // duplicate Port no
            var duplicatePortNo = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.port_no))
                .GroupBy(x => x.port_no).Where(x => x.Count() > 1).ToList();

            if (duplicatePortNo.Count > 0)
            {
                foreach (var item in duplicatePortNo)
                {
                    report.Add(new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.DuplicateField, nameof(ImportPortData.port_no),
                            string.Join(",", item.Select(x => x.index)))
                    });

                    invalidRows.AddRange(item.Select(x => x.index).ToList());
                }
            }

            // Port no too long
            var maxLengthOfIPortNo = Utilities.GetMaxLength(typeof(Port), nameof(Port.port_no)) ?? 50;
            var portNoTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.port_no) &&
                            x.port_no.Length > maxLengthOfIPortNo).ToList();
            if (portNoTooLongFromExcel.Count > 0)
            {
                foreach (var item in portNoTooLongFromExcel)
                {
                    report.Add(new ImportReportDto()
                    {
                        Identifier = item.port_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Port.port_no), maxLengthOfIPortNo, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region Port name
            // missing Port name
            var missingSupplierName = request.ExcelData.Where(x => string.IsNullOrEmpty(x.port_name)).ToList();
            if (missingSupplierName.Count > 0)
            {
                report.Add(new ImportReportDto
                {
                    Message = string.Format(ErrorMessages.MissingField, nameof(ImportPortData.port_name),
                        string.Join(",", missingSupplierName.Select(x => x.index)))
                });

                invalidRows.AddRange(missingSupplierName.Select(x => x.index).ToList());
            }

            // Port name too long
            var maxLengthOfISupplierName = Utilities.GetMaxLength(typeof(Port), nameof(Port.port_name)) ?? 100;
            var portNameTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.port_name) && x.port_name.Length > maxLengthOfISupplierName).ToList();

            if (portNameTooLongFromExcel.Count > 0)
            {
                foreach (var item in portNameTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.port_name,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(Port.port_name), maxLengthOfISupplierName, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }
            #endregion

            #region County code
            var maxLengthOfICounty = Utilities.GetMaxLength(typeof(Country), nameof(Country.country_alpha_code_two)) ?? 2;
            var countyTooLongFromExcel = request.ExcelData
                .Where(x => !string.IsNullOrEmpty(x.country_code) && x.country_code.Length > maxLengthOfICounty).ToList();

            if (countyTooLongFromExcel.Count > 0)
            {
                foreach (var item in countyTooLongFromExcel)
                {
                    report.Add(new ImportReportDto
                    {
                        Identifier = item.port_no,
                        Message = string.Format(ErrorMessages.ValueTooLongFromExcel, nameof(ImportPortData.country_code),
                            maxLengthOfICounty, string.Join(",", item.index))
                    });

                    invalidRows.Add(item.index);
                }
            }

            var invalidCountryCodes = request.ExcelData.Where(x => !string.IsNullOrEmpty(x.country_code))
                .Select(x => x.country_code).Except(request.Countries.Select(y => y.country_alpha_code_two)).Distinct().ToList();

            if (invalidCountryCodes.Count > 0)
            {
                foreach (var item in invalidCountryCodes)
                {
                    var invalidItem = request.ExcelData.Where(x => x.country_code == item).ToList();
                    report.Add(new ImportReportDto
                    {
                        Message = string.Format(ErrorMessages.NotExistedInDatabase, nameof(ImportPortData.country_code), item, string.Join(",", invalidItem.Select(x => x.index)))
                    });

                    invalidRows.AddRange(invalidItem.Select(x => x.index));
                }
            }
            #endregion

            return new Tuple<List<int>, List<ImportReportDto>>(invalidRows.Distinct().ToList(), report);
        }

        private async Task<List<ImportPortData>> ReadExcelPortDataAsync(string inputFilePath)
        {
            var excludedProperties = new HashSet<string> { "index" };
            var result = new List<ImportPortData>();
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
                var rowData = new ImportPortData
                {
                    index = rowIndex++,

                    country_code = item[PortExcelColumns.CountryCode] != DBNull.Value
                        ? item[PortExcelColumns.CountryCode].ToString()
                        : null,

                    port_no = item[PortExcelColumns.PortNo] != DBNull.Value
                        ? item[PortExcelColumns.PortNo].ToString()
                        : null,

                    port_name = item[PortExcelColumns.PortName] != DBNull.Value
                        ? item[PortExcelColumns.PortName].ToString()
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
    }
}
