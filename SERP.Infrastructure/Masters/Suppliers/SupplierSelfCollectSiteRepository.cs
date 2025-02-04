using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.Suppliers.Interfaces;
using SERP.Domain.Masters.Suppliers;
using SERP.Domain.Masters.Suppliers.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Suppliers
{
    internal class SupplierSelfCollectSiteRepository: GenericRepository<SupplierSelfCollectSite>, ISupplierSelfCollectSiteRepository
    {
        public SupplierSelfCollectSiteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<SupplierSelfCollectSitePagedResponseDetail> BuildSelfCollectSiteFilterQuery(FilterModel request)
        {
            var supplierSelfCollectSiteQuery = _dbContext.SupplierSelfCollectSites.AsNoTracking()
            .Where(x => x.supplier_id == request.supplier_id);

            if (request.create_date_from.HasValue)
            {
                supplierSelfCollectSiteQuery = supplierSelfCollectSiteQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                supplierSelfCollectSiteQuery = supplierSelfCollectSiteQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            if (request.status_flag is not null && request.status_flag.Count > 0)
            {
                supplierSelfCollectSiteQuery = supplierSelfCollectSiteQuery.Where(x => request.status_flag.Contains(x.status_flag));
            }

            var query = from supplierSelfCollectSite in supplierSelfCollectSiteQuery
                        join supplier in _dbContext.Supplier.AsNoTracking() on supplierSelfCollectSite.supplier_id equals supplier.id
                        join site in _dbContext.Site.AsNoTracking() on supplierSelfCollectSite.site_id equals site.id
                        select new SupplierSelfCollectSitePagedResponseDetail
                        {
                            id = supplierSelfCollectSite.id,
                            self_collect_site_id = supplierSelfCollectSite.site_id,
                            site_no = site.site_no,
                            site_name = site.site_name,
                            status_flag = supplierSelfCollectSite.status_flag,
                            created_on = supplierSelfCollectSite.created_on,
                            created_by = supplierSelfCollectSite.created_by,
                            last_modified_on = supplierSelfCollectSite.last_modified_on,
                            last_modified_by = supplierSelfCollectSite.last_modified_by
                        };

            // - Keywords are Site No and Site Name
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x =>
                                    x.site_no.Contains(request.Keyword) ||
                                    x.site_name.Contains(request.Keyword));
            }

            return query;
        }
    }
}
