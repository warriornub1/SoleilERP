using SERP.Application.Common;
using SERP.Domain.Transactions.CustomViews;

namespace SERP.Application.Transactions.CustomViews.Interfaces
{
    public interface ICustomViewFilterRepository: IGenericRepository<CustomViewFilter>
    {
        Task<List<CustomViewFilter>> GetByCustomViewId(List<int> customViewIds);
    }
}
