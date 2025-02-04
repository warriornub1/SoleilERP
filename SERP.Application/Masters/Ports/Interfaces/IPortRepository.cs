using SERP.Application.Common;
using SERP.Domain.Masters.Ports;
using SERP.Domain.Masters.Ports.Model;

namespace SERP.Application.Masters.Ports.Interfaces
{
    public interface IPortRepository : IGenericRepository<Port>
    {
        Task<List<PortDetail>> GetByCountryCode(string? countryCode, int? countryId);
        Task<int[]> CheckInvalidPortIds(HashSet<int> portIds);
        Task<Dictionary<string, Port>> GetDictionaryByPortNoAsync(List<string> toList);
        Task<List<Port>> GetPortListAsync(HashSet<string> portNo);
        Task<int[]> GetPortAvailable(HashSet<int> portOfDischargeIds);
        Task<int?> GetPortCountryIdAsync(int? portId);
    }
}
