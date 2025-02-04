using Microsoft.EntityFrameworkCore;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System.Linq.Expressions;

namespace SERP.Infrastructure.Masters.Suppliers
{
    public class SupplierSecondaryRepository : GenericRepository<SecondarySupplier>, ISupplierSecondaryRepository
    {
        public SupplierSecondaryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<SupplierSecondaryPagedResponseDetail> BuildSupplierSecondaryFilterQuery(FilterModel request)
        {
            var secondarySupplierQuery = _dbContext.SecondarySupplier.AsNoTracking()
            .Where(x => x.supplier_id == request.supplier_id);

            if (request.default_flag.HasValue)
            {
                secondarySupplierQuery = secondarySupplierQuery.Where(x => x.default_flag == request.default_flag.Value);
            }

            if (request.create_date_from.HasValue)
            {
                secondarySupplierQuery = secondarySupplierQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                secondarySupplierQuery = secondarySupplierQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                secondarySupplierQuery = secondarySupplierQuery.Where(x => request.status_flag.Contains(x.status_flag));
            }

            var query = from secondarySupplier in secondarySupplierQuery
                        select new SupplierSecondaryPagedResponseDetail
                        {
                            secondary_supplier_id = secondarySupplier.id,
                            secondary_supplier_no = secondarySupplier.sec_supplier_no,
                            secondary_supplier_name = secondarySupplier.sec_supplier_name,
                            status_flag = secondarySupplier.status_flag,
                            default_flag = secondarySupplier.default_flag,
                            created_on = secondarySupplier.created_on,
                            created_by = secondarySupplier.created_by,
                            last_modified_on = secondarySupplier.last_modified_on,
                            last_modified_by = secondarySupplier.last_modified_by
                        };

            // - Keywords are Secondary Supplier No, Secondary Supplier Name
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                    x.secondary_supplier_no.Contains(request.Keyword) ||
                    x.secondary_supplier_name.Contains(request.Keyword));
            }

            return query;
        }

        public async Task<Dictionary<Tuple<int, string>, SecondarySupplier>> GetDictionarySupplierSecondaryAsync(Expression<Func<SecondarySupplier, bool>> lambda)
        {
            return await _dbContext.SecondarySupplier
                .Where(lambda)
                .ToDictionaryAsync(x => new Tuple<int, string>(x.supplier_id, x.sec_supplier_no));
        }

        public async Task<int[]> GetAvailableSecondarySupplier(HashSet<int> secondarySupplierIds)
        {
            return await _dbContext.SecondarySupplier
                .Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled &&
                           secondarySupplierIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }
    }
}
