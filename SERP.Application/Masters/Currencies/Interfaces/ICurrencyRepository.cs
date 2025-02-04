using SERP.Application.Common;
using SERP.Domain.Masters.Currencies;
using SERP.Domain.Masters.Currencies.Model;

namespace SERP.Application.Masters.Currencies.Interfaces
{
    public interface ICurrencyRepository: IGenericRepository<Currency>
    {
        Task<List<CurrencyDetail>> GetAllLimited(bool onlyEnabled);
        Task<CurrencyExchangeDetail?> GetExchangeRate(int fromCurrencyId, int toCurrencyId);
        Task<int[]> GetCurrencyAvailable(HashSet<int> currencyIDs);
        Task<bool> FindCurrency(int base_currency_id);
        Task<List<Currency>> GetCurrencyListAsync(HashSet<string> currencyCode);
        Task<IEnumerable<int>> GetIdWithFlag(List<int> currencyId, string flag); 
        Task<Dictionary<string, int>> GetAllCurrencyIdDictionary(List<string?> base_currency);
    }
}
