using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SERP.Application.Masters.Countries.Interfaces;
using SERP.Domain.Masters.Countries;
using SERP.Domain.Masters.Countries.Models;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;

namespace SERP.Infrastructure.Masters.Countries
{
    internal class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<CountryDetail>> GetAllLimited()
        {
            return await _dbContext.Country.Select(x => new CountryDetail
            {
                country_id = x.id,
                country_name = x.country_name,
                country_alpha_code_two = x.country_alpha_code_two,
                country_alpha_code_three = x.country_alpha_code_three,
                country_idd = x.country_idd
            }).ToListAsync();
        }

        public async Task<Dictionary<string, Country>> GetByCountryCode2DigitsAsync(List<string> validCountryCode2Digits)
        {
            return await _dbContext.Country.Where(x => validCountryCode2Digits.Contains(x.country_alpha_code_two))
                .ToDictionaryAsync(x => x.country_alpha_code_two);

        }

        public async Task<List<Country>> GetCountryCodeListAsync(HashSet<string> countryCodes)
        {
            return await _dbContext.Country.Where(x => countryCodes.Contains(x.country_alpha_code_two))
                .Select(x => new Country()
                {
                    id = x.id,
                    country_alpha_code_two = x.country_alpha_code_two
                })
                .ToListAsync();
        }

        public async Task<bool> CountryExistsAsync(int countryId)
        {
            return await _dbContext.Country.AnyAsync(x => x.id == countryId);
        }
    }
}
