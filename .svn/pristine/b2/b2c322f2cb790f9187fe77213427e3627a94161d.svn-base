using SERP.Application.Common;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using System.Linq.Expressions;

namespace SERP.Application.Masters.Suppliers.Interfaces
{
    public interface ISupplierSecondaryRepository: IGenericRepository<SecondarySupplier>
    {
        IQueryable<SupplierSecondaryPagedResponseDetail> BuildSupplierSecondaryFilterQuery(FilterModel filterModel);
        Task<Dictionary<Tuple<int, string>, SecondarySupplier>> GetDictionarySupplierSecondaryAsync(Expression<Func<SecondarySupplier, bool>> lambda);
        Task<int[]> GetAvailableSecondarySupplier(HashSet<int> secondarySupplierIds);
    }
}
