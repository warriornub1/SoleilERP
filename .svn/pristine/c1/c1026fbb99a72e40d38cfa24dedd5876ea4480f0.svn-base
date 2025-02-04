using SERP.Application.Common;
using SERP.Domain.Transactions.FilesTracking;

namespace SERP.Application.Transactions.FilesTracking.Interfaces
{
    public interface IFileTrackingRepository : IGenericRepository<FileTracking>
    {
        Task<bool> IsFileExists(string fileName);
        Task<string[]> ValidateFileNameInDatabase(string[] fileNames);
        Task<List<FileTracking>> GetFileTrackingByIds(HashSet<int> listFileTrackingIDs);
    }
}
