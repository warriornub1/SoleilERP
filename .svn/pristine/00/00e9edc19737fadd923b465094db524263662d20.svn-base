using SERP.Application.Common;
using SERP.Domain.Transactions.CustomViews;
using SERP.Domain.Transactions.CustomViews.Model;

namespace SERP.Application.Transactions.CustomViews.Interfaces
{
    public interface ICustomViewRepository : IGenericRepository<CustomView>
    {
        Task<IEnumerable<CustomViewDetail>> GetByCustomViewType(string customViewType, string? userId, bool onlyEnabled);
        Task<CustomViewAttributeDetail?> GetAttributesByCustomViewId(int customViewId);
        Task<bool> IsCustomViewNameExist(string customViewName, string customViewType, string? userId);
        IQueryable<PageCustomViewResponseDetail> BuildFilterCustomViewQuery(PagedFilterCustomViewRequestModel pagedFilterCustomViewRequestModel);
    }
}
