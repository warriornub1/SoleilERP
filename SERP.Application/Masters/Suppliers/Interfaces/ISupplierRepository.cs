using SERP.Application.Common;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;

namespace SERP.Application.Masters.Suppliers.Interfaces
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> GetAllLimited(bool onlyEnabled);
        Task<SupplierDetail?> GetById(int id);
        Task<IEnumerable<SecondarySupplier>> GetSecondarySupplierLimited(int supplierId, bool onlyEnabled);
        Task<IEnumerable<SupplierItemMapping>> GetSupplierItemMapping(int supplierId, int itemId, bool onlyEnabled);
        Task<int[]> CheckInvalidSupplierIds(List<int> supplierIds);
        Task<List<IntermediarySupplier>> GetIntermediarySupplierList(HashSet<int> supplierIds);
        Task<List<Supplier>> GetSupplierNo(HashSet<int> supplierIDs);
        Task<int[]> GetSupplierAvailable(HashSet<int> supplierListIds);
        IQueryable<PagedSupplierDetail> BuildSupplierFilterQuery(PagedFilterSupplierRequestModel request, out int totalRows);
        Task<string[]> CheckInvalidSupplierNOs(List<Supplier> suppliers);
        Task<string[]> CheckExistingSupplierNo(List<string> supplierNOs);
    }
}
