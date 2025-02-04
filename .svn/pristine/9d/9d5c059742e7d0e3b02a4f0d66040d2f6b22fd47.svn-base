using SERP.Application.Common;
using SERP.Application.Masters.Lovs.DTOs.Request;
using SERP.Domain.Masters.LOVs;
using SERP.Domain.Masters.LOVs.Model;
using System.Linq.Expressions;

namespace SERP.Application.Masters.Lovs.Interfaces
{
    public interface ILovRepository : IGenericRepository<Lov>
    {
        Task<IEnumerable<Lov>> GetByLovTypeAsync(List<string> lovTypes, bool onlyEnabled);
        IQueryable<Lov> LovFilterAsync(FilterLovRequestModel request);

        Task<List<string>> FindLovRecords(List<(string, bool)> requests);

        Task<Lov> GetFirst(Expression<Func<Lov, bool>> expression);

        Task<List<string>> FindLovRecordsWithID(List<(string, bool, int)> requests);
        Task<HashSet<string>> FindNaturalAccountType(List<string> naturalAccount, string LovTypes);
    }
}
