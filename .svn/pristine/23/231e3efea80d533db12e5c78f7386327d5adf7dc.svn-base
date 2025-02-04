using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Suppliers
{
    internal class IntermediarySupplierRepository:GenericRepository<IntermediarySupplier>, IIntermediarySupplierRepository
    {
        public IntermediarySupplierRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<IntermediarySupplierPagedResponseDetail> BuildSupplierIntermediaryFilterQuery(FilterModel request)
        {
            var intermediarySupplierQuery = _dbContext.IntermediarySupplier.AsNoTracking()
            .Where(x => x.supplier_id == request.supplier_id);

            if (request.default_flag.HasValue)
            {
                intermediarySupplierQuery = intermediarySupplierQuery.Where(x => x.default_flag == request.default_flag.Value);
            }

            if (request.create_date_from.HasValue)
            {
                intermediarySupplierQuery = intermediarySupplierQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                intermediarySupplierQuery = intermediarySupplierQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                intermediarySupplierQuery = intermediarySupplierQuery.Where(x => request.status_flag.Contains(x.status_flag));
            }

            var query = from intermediarySupplier in intermediarySupplierQuery
                        join supplier in _dbContext.Supplier.AsNoTracking() on intermediarySupplier.int_supplier_id equals supplier.id
                        select new IntermediarySupplierPagedResponseDetail
                        {
                            id = intermediarySupplier.id,
                            intermediary_supplier_id = intermediarySupplier.int_supplier_id,
                            intermediary_supplier_no = supplier.supplier_no,
                            intermediary_supplier_name = supplier.supplier_name,
                            default_flag = intermediarySupplier.default_flag,
                            status_flag = intermediarySupplier.status_flag,
                            created_on = intermediarySupplier.created_on,
                            created_by = intermediarySupplier.created_by,
                            last_modified_on = intermediarySupplier.last_modified_on,
                            last_modified_by = intermediarySupplier.last_modified_by
                        };

            // - Keywords are Intermediary Supplier No, Intermediary Supplier Name
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                                    x.intermediary_supplier_no.Contains(request.Keyword) ||
                                    x.intermediary_supplier_name.Contains(request.Keyword));
            }

            return query;
        }
    }
}
