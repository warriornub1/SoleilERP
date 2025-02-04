using Microsoft.Extensions.Caching.Memory;
using SERP.Application.Transactions.PurchaseOrders.Interfaces;
using SERP.Domain.Transactions.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Application.Transactions.Test
{
    public class TestService : ITestService
    {
        private readonly IPODetailRepository _poDetailRepository;
        private readonly IMemoryCache _memoryCache;
        private string Key = "test";


        public TestService(IPODetailRepository poDetailRepository,
            IMemoryCache memoryCache)
        {
            _poDetailRepository = poDetailRepository;
            _memoryCache = memoryCache;
        }
        public async Task<(string, IEnumerable<PODetail>)> TestGetAllPODetail()
        {
            string inCache = "In cache";
            IEnumerable<PODetail> result;
            _memoryCache.TryGetValue(Key, out result);

            if (result == null)
            {
                inCache = "Not in cache";
                result = await _poDetailRepository.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(120))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(Key, result, cacheEntryOptions);
            };

            return (inCache, result);
        }
    }
}
