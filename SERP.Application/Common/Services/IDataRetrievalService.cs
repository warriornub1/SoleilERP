using SERP.Domain.Common.Enums;

namespace SERP.Application.Common.Services
{
    public interface IDataRetrievalService
    {
        Task<Dictionary<SERPEnum.DictionaryType, Dictionary<int, List<string>>>> FetchAllDictionariesForGetDetailAsync(
            List<int>? supplierIds = null, List<int>? secondarySupplierIds = null, List<int>? siteIds = null, List<int>? portIds = null, List<int>? currencyIds = null, List<int>? agentIds = null,
            List<int>? branchPlantIds = null, List<int>? companyIds = null, List<int>? itemIds = null, List<int>? supplierItemMappingIds = null,
            List<int>? countryIds = null);

        Task<byte[]> GetTemplateAsync(string webRootPath, string fileName);
    }
}