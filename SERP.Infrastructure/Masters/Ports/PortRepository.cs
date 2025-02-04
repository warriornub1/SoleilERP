using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.Ports.Interfaces;
using SERP.Domain.Common.Constants;
using SERP.Domain.Masters.Countries.Models;
using SERP.Domain.Masters.Ports;
using SERP.Domain.Masters.Ports.Model;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Ports
{
    public class PortRepository : GenericRepository<Port>, IPortRepository
    {
        public PortRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PortDetail>> GetByCountryCode(string? countryCode, int? countryId)
        {
            var countryQuery = _dbContext.Country.AsNoTracking();

            if (!string.IsNullOrEmpty(countryCode))
            {
                countryQuery = countryQuery.Where(x => x.country_alpha_code_two.Equals(countryCode));
            }

            if (countryId.HasValue && countryId != 0)
            {
                countryQuery = countryQuery.Where(x => x.id == countryId);
            }

            var ports = await (from port in _dbContext.Ports.AsNoTracking()
                               join country in countryQuery on port.country_id equals country.id
                               select new PortDetail
                               {
                                   id = port.id,
                                   port_no = port.port_no,
                                   port_name = port.port_name,
                                   status_flag = port.status_flag,
                                   country = new CountryDetail
                                   {
                                       country_id = country.id,
                                       country_name = country.country_name,
                                       country_long_name = country.country_long_name,
                                       country_alpha_code_two = country.country_alpha_code_two,
                                       country_alpha_code_three = country.country_alpha_code_three,
                                       country_idd = country.country_idd,
                                       continent = country.continent
                                   }
                               }).ToListAsync();

            return ports;
        }

        public async Task<int[]> CheckInvalidPortIds(HashSet<int> portIds)
        {
            var query = await _dbContext.Ports
                .Where(x => portIds.Contains(x.id) && x.status_flag.Equals(DomainConstant.StatusFlag.Enabled))
                .Select(x => x.id).ToArrayAsync();
            var result = portIds.Except(query);
            return result.ToArray();
        }

        public async Task<Dictionary<string, Port>> GetDictionaryByPortNoAsync(List<string> portNos)
        {
            return await _dbContext.Ports
                .Where(x => portNos.Contains(x.port_no))
                .ToDictionaryAsync(x => x.port_no);
        }

        public async Task<List<Port>> GetPortListAsync(HashSet<string> portNo)
        {
            return await  _dbContext.Ports
                .Where(x => portNo.Contains(x.port_no))
                .Select(x => new Port
                {
                    id = x.id,
                    port_no = x.port_no
                })
                .ToListAsync();
        }

        public async Task<int[]> GetPortAvailable(HashSet<int> portOfDischargeIds)
        {
            return await _dbContext.Ports
                .Where(x => portOfDischargeIds.Contains(x.id))
                .Select(x => x.id).ToArrayAsync();
        }

        public async Task<int?> GetPortCountryIdAsync(int? portId)
        {
            return await _dbContext.Ports
                .Where(x => x.id == portId)
                .Select(x => x.country_id).FirstOrDefaultAsync();
        }
    }
}
