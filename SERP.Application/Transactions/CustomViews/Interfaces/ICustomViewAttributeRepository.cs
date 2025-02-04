using SERP.Application.Common;
using SERP.Domain.Transactions.CustomViews;

namespace SERP.Application.Transactions.CustomViews.Interfaces
{
    public interface ICustomViewAttributeRepository : IGenericRepository<CustomViewAttribute>
    {
        Task<List<CustomViewAttribute>> GetByCustomViewId(List<int> customViewIds);
    }
}
