using SERP.Application.Common;
using SERP.Domain.Transactions.SequencesTracking;

namespace SERP.Application.Transactions.SequencesTracking.Interfaces
{
    public interface ISequenceTrackingRepository : IGenericRepository<SequenceTracking>
    {
        Task<int> GetSequenceNoByType(string type);
    }
}
