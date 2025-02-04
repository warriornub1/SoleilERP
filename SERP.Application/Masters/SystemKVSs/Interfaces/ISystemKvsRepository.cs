using SERP.Application.Common;
using SERP.Domain.Masters.SystemKVSs;
using SERP.Domain.Masters.SystemKVSs.Model;

namespace SERP.Application.Masters.SystemKVSs.Interfaces
{
    public interface ISystemKvsRepository : IGenericRepository<SystemKvs>
    {
        Task<SystemKvs> GetSystemKvsByKeywordAsync(string keyword);
        Task<FileUploadLimitedDetail?> GetFileUploadLimited();
    }
}
