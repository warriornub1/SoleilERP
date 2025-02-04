using Microsoft.Extensions.Logging;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.Agents.Interfaces;
using SERP.Application.Masters.BranchPlants.Interfaces;
using SERP.Application.Masters.Companies.Interfaces;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Application.Masters.Currencies.Interfaces;
using SERP.Application.Masters.Items.Interfaces;
using SERP.Application.Masters.Ports.Interfaces;
using SERP.Application.Masters.Sites.Interfaces;
using SERP.Application.Masters.Suppliers.Interfaces;
using static SERP.Domain.Common.Enums.SERPEnum;

namespace SERP.Application.Common.Services
{
    internal class DataRetrievalService : IDataRetrievalService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IBranchPlantRepository _branchPlantRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPortRepository _portRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierSecondaryRepository _supplierSecondaryRepository;
        private readonly ISupplierItemMappingRepository _supplierItemMappingRepository;
        private readonly ILogger<DataRetrievalService> _logger;

        public DataRetrievalService(
            IAgentRepository agentRepository,
            IBranchPlantRepository branchPlantRepository,
            ICompanyRepository companyRepository,
            ICountryRepository countryRepository,
            ICurrencyRepository currencyRepository,
            IPortRepository portRepository,
            ISiteRepository siteRepository,
            IItemRepository itemRepository,
            ISupplierRepository supplierRepository,
            ISupplierSecondaryRepository supplierSecondaryRepository,
            ISupplierItemMappingRepository supplierItemMappingRepository,
            ILogger<DataRetrievalService> logger)
        {
            _agentRepository = agentRepository;
            _branchPlantRepository = branchPlantRepository;
            _companyRepository = companyRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _portRepository = portRepository;
            _siteRepository = siteRepository;
            _itemRepository = itemRepository;
            _supplierRepository = supplierRepository;
            _supplierSecondaryRepository = supplierSecondaryRepository;
            _supplierItemMappingRepository = supplierItemMappingRepository;
            _logger = logger;
        }

        public async Task<Dictionary<DictionaryType, Dictionary<int, List<string>>>>
            FetchAllDictionariesForGetDetailAsync(
                List<int>? supplierIds,
                List<int>? secondarySupplierIds,
                List<int>? siteIds,
                List<int>? portIds,
                List<int>? currencyIds,
                List<int>? agentIds,
                List<int>? branchPlantIds,
                List<int>? companyIds,
                List<int>? itemIds,
                List<int>? supplierItemMappingIds,
                List<int>? countryIds)
        {
            var resultDict = new Dictionary<DictionaryType, Dictionary<int, List<string>>>
            {
                [DictionaryType.Supplier] = await GetSupplierDictionaryAsync(new HashSet<int>(supplierIds?.Distinct() ?? [])),
                [DictionaryType.SecondarySupplier] = await GetSecondarySupplierDictionaryAsync(new HashSet<int>(secondarySupplierIds?.Distinct() ?? [])),
                [DictionaryType.Site] = await GetSiteDictionaryAsync(new HashSet<int>(siteIds?.Distinct() ?? [])),
                [DictionaryType.Port] = await GetPortDictionaryAsync(new HashSet<int>(portIds?.Distinct() ?? [])),
                [DictionaryType.Currency] = await GetCurrencyDictionaryAsync(new HashSet<int>(currencyIds?.Distinct() ?? [])),
                [DictionaryType.Agent] = await GetAgentDictionaryAsync(new HashSet<int>(agentIds?.Distinct() ?? [])),
                [DictionaryType.BranchPlant] =
                    await GetBranchPlantDictionaryAsync(new HashSet<int>(branchPlantIds?.Distinct() ?? [])),
                [DictionaryType.Company] =
                    await GetCompanyDictionaryAsync(new HashSet<int>(companyIds?.Distinct() ?? [])),
                [DictionaryType.Item] = await GetItemDictionaryAsync(new HashSet<int>(itemIds?.Distinct() ?? [])),
                [DictionaryType.SupplierItemMapping] =
                    await GetSupplierItemMappingDictionaryAsync(new HashSet<int>(supplierItemMappingIds?.Distinct() ?? [])),
                [DictionaryType.Country] = await GetCountryDictionaryAsync(new HashSet<int>(countryIds?.Distinct() ?? []))
            };

            return resultDict;
        }

        private async Task<Dictionary<int, List<string>>> GetCountryDictionaryAsync(HashSet<int>? countryIds)
        {
            if (countryIds is null || countryIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _countryRepository.GetQuery()
                    .Where(x => countryIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.country_alpha_code_two,
                    y.country_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetSupplierDictionaryAsync(HashSet<int>? supplierIds)
        {
            if (supplierIds is null || supplierIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _supplierRepository.GetQuery()
                    .Where(x => supplierIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.supplier_no,
                    y.supplier_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetSecondarySupplierDictionaryAsync(HashSet<int>? secondarySupplierIds)
        {
            if (secondarySupplierIds is null || secondarySupplierIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _supplierSecondaryRepository.GetQuery()
                    .Where(x => secondarySupplierIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.sec_supplier_no,
                    y.sec_supplier_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetSiteDictionaryAsync(HashSet<int>? siteIds)
        {
            if (siteIds is null || siteIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _siteRepository.GetQuery()
                    .Where(x => siteIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.site_no,
                    y.site_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetPortDictionaryAsync(HashSet<int>? portIds)
        {
            if (portIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _portRepository.GetQuery()
                    .Where(x => portIds.Where(id => id != 0).Contains(x.id)),
            x => x.id,
                y => new List<string>
                {
                    y.port_no,
                    y.port_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetCurrencyDictionaryAsync(HashSet<int>? currencyIds)
        {
            if (currencyIds is null || currencyIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _currencyRepository.GetQuery()
                    .Where(x => currencyIds.Where(id => id != 0).Contains(x.id)),
            x => x.id,
                y => new List<string>
                {
                    y.currency_code,
                    y.status_flag
                });
        }

        private async Task<Dictionary<int, List<string>>> GetAgentDictionaryAsync(HashSet<int>? agentIds)
        {
            if (agentIds is null || agentIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _agentRepository.GetQuery()
                    .Where(x => agentIds.Where(id => id != 0).Contains(x.id)),
            x => x.id,
                y => new List<string>
                {
                    y.agent_no,
                    y.agent_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetBranchPlantDictionaryAsync(HashSet<int>? branchPlantIds)
        {
            if (branchPlantIds is null || branchPlantIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _branchPlantRepository.GetQuery()
                    .Where(x => branchPlantIds.Where(id => id != 0).Contains(x.id)),
            x => x.id,
                y => new List<string>
                {
                    y.branch_plant_no,
                    y.branch_plant_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetCompanyDictionaryAsync(HashSet<int>? companyIds)
        {
            if (companyIds is null || companyIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _companyRepository.GetQuery()
                    .Where(x => companyIds.Where(id => id != 0).Contains(x.id)),
            x => x.id,
                y => new List<string>
                {
                    y.company_no,
                    y.company_name
                });
        }

        private async Task<Dictionary<int, List<string>>> GetItemDictionaryAsync(HashSet<int>? itemIds)
        {
            if (itemIds is null || itemIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _itemRepository.GetQuery()
                    .Where(x => itemIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.item_no,
                    y.description_1,
                    y.description_2 ?? string.Empty,
                    y.primary_uom,
                    y.secondary_uom
                });
        }

        private async Task<Dictionary<int, List<string>>> GetSupplierItemMappingDictionaryAsync(
            HashSet<int>? supplierItemMappingIds)
        {
            if ( supplierItemMappingIds is null || supplierItemMappingIds.Count == 0) return new Dictionary<int, List<string>>();

            return await Utilities.GetDictionaryAsync(
                _supplierItemMappingRepository.GetQuery()
                    .Where(x => supplierItemMappingIds.Where(id => id != 0).Contains(x.id)),
                x => x.id,
                y => new List<string>
                {
                    y.supplier_part_no ?? string.Empty,
                    y.supplier_material_code ?? string.Empty,
                    y.supplier_material_description ?? string.Empty
                });
        }

        public async Task<byte[]> GetTemplateAsync(string webRootPath, string fileName)
        {
            string filePath = Path.Combine(webRootPath, fileName);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new BadRequestException(ErrorCodes.FileNotFound, ErrorMessages.FileNotFound);

            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            return fileBytes;
        }

    }
}
