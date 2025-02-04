using SERP.Application.Common;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;

namespace SERP.Application.Masters.Suppliers.Interfaces
{
    public interface ISupplierItemMappingRepository: IGenericRepository<SupplierItemMapping>
    {
        Task<Dictionary<Tuple<int, int, string?>, SupplierItemMapping>> GetDictionarySupplierItemMappingAsync(HashSet<int> supplierIds, HashSet<int> itemIds, HashSet<string?> supplierMaterialCodes);
        Task<int[]> GetSupplierItemMappingAvailable(HashSet<int> supplierMappingIds);
        IQueryable<SupplierItemMappingPagedResponseDetail> BuildSupplierItemMappingFilterQuery(FilterModel filterModel);
    }
}
