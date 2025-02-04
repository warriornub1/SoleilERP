using SERP.Application.Masters.Currencies.Interfaces;
using SERP.Domain.Masters.Currencies;
using SERP.Infrastructure.Common;
using SERP.Infrastructure.Common.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Infrastructure.Masters.Currencies
{

    public class CurrencyExchangeRepository : GenericRepository<CurrencyExchange>, ICurrencyExchangeRepository
    {
        public CurrencyExchangeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
