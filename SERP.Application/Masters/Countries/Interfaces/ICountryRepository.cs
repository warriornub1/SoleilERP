using SERP.Application.Common;
using SERP.Domain.Masters.Countries;
using SERP.Domain.Masters.Countries.Models;

namespace SERP.Application.Masters.Countries.Interfaces
{
    public interface ICountryRepository: IGenericRepository<Country>
    {
        Task<List<CountryDetail>> GetAllLimited();
        Task<Dictionary<string, Country>> GetByCountryCode2DigitsAsync(List<string> validCountryCode2Digits);
        Task<List<Country>> GetCountryCodeListAsync(HashSet<string> countryCodes);
        Task<bool> CountryExistsAsync(int countryId);
    }
}
