using SERP.Application.Common;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;

namespace SERP.Application.Masters.Suppliers.Interfaces
{
    public interface IIntermediarySupplierRepository: IGenericRepository<IntermediarySupplier>
    {
        IQueryable<IntermediarySupplierPagedResponseDetail> BuildSupplierIntermediaryFilterQuery(FilterModel filterModel);
    }
}
