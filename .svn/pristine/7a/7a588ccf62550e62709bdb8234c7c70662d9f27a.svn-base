using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Transactions.CustomViews.Interfaces;
using SERP.Domain.Transactions.CustomViews;
using SERP.Domain.Transactions.CustomViews.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Transactions.CustomViews
{
    internal class CustomViewRepository : GenericRepository<CustomView>, ICustomViewRepository
    {
        public CustomViewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CustomViewDetail>> GetByCustomViewType(string customViewType, string? userId, bool onlyEnabled)
        {
            var query = from customView in _dbContext.CustomView
                        where customView.custom_view_type == customViewType
                        select customView;

            if (onlyEnabled)
            {
                query = query.Where(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled);
            }

            // - Return only records with private_flag 0: False
            // - If user id is given, return records with private_flag 1: True too but only for the given user id
            query = !string.IsNullOrEmpty(userId) ?
                    query.Where(x => (x.private_flag == true && x.user_id == userId) || x.private_flag == false) :
                    query.Where(x => x.private_flag == false);

            return await query.Select(customView => new CustomViewDetail
            {
                id = customView.id,
                custom_view_type = customView.custom_view_type,
                custom_view_name = customView.custom_view_name,
                private_flag = customView.private_flag,
                default_flag = customView.default_flag,
                allow_update_delete_flag = customView.allow_update_delete_flag,
                status_flag = Utilities.ParseBool(customView.status_flag)
            }).ToListAsync();
        }

        public async Task<CustomViewAttributeDetail?> GetAttributesByCustomViewId(int customViewId)
        {
            var result = await (from customView in _dbContext.CustomView
                                where customView.id == customViewId
                                select new CustomViewAttributeDetail
                                {
                                    custom_view_id = customView.id,
                                    custom_view_name = customView.custom_view_name,
                                    attributes = (from customViewAttribute in _dbContext.CustomViewAttributes
                                                  where customViewAttribute.custom_view_id == customView.id
                                                  orderby customViewAttribute.seq_no
                                                  select new AttributeDetail
                                                  {
                                                      custom_view_attribute_id = customViewAttribute.id,
                                                      attribute = customViewAttribute.attribute,
                                                      attribute_type = customViewAttribute.attribute_type,
                                                      seq_no = customViewAttribute.seq_no,
                                                      column_freeze_flag = customViewAttribute.column_freeze_flag,
                                                      sort_direction = customViewAttribute.sort_direction
                                                  }).ToList(),
                                    filters = (from customViewFilter in _dbContext.CustomViewFilters
                                               where customViewFilter.custom_view_id == customView.id
                                               select new CustomViewFilterDetail
                                               {
                                                   custom_view_filter_id = customViewFilter.id,
                                                   filter = customViewFilter.filter,
                                                   filter_value = customViewFilter.filter_value
                                               }).ToList()
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> IsCustomViewNameExist(string customViewName, string customViewType, string? userId)
        {
            var query = _dbContext.CustomView.Where(x => x.custom_view_name == customViewName && x.custom_view_type == customViewType);
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.user_id == userId);
            }

            return await query.AnyAsync();
        }

        public IQueryable<PageCustomViewResponseDetail> BuildFilterCustomViewQuery(PagedFilterCustomViewRequestModel request)
        {
            var customViewQuery = _dbContext.CustomView.AsNoTracking();

            if (request.custom_view_type is not null && request.custom_view_type.Count > 0)
            {
                customViewQuery = customViewQuery.Where(x => request.custom_view_type.Contains(x.custom_view_type));
            }

            if (request.create_date_from.HasValue)
            {
                customViewQuery = customViewQuery.Where(x => x.created_on >= request.create_date_from.Value);
            }

            if (request.create_date_to.HasValue)
            {
                customViewQuery = customViewQuery.Where(x => x.created_on <= request.create_date_to.Value);
            }

            var response = customViewQuery.Select(x => new PageCustomViewResponseDetail
            {
                id = x.id,
                custom_view_type = x.custom_view_type,
                custom_view_name = x.custom_view_name,
                private_flag = x.private_flag,
                allow_update_delete_flag = x.allow_update_delete_flag,
                user_id = x.user_id,
                default_flag = x.default_flag,
                status_flag = x.status_flag,
                created_on = x.created_on,
                created_by = x.created_by,
                last_modified_on = x.last_modified_on,
                last_modified_by = x.last_modified_by
            });

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                response = response.Where(x => x.custom_view_name.Contains(request.Keyword));
            }

            return response;
        }
    }
}
