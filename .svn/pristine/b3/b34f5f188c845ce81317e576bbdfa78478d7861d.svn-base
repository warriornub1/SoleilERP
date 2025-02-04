using Microsoft.AspNetCore.Mvc;
using SERP.Application.Masters.Currencies.Services;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CurrencyController: ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllLimited(bool onlyEnabled)
        {
            var customers = await _currencyService.GetAllLimited(onlyEnabled);
            return Ok(customers);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{fromCurrencyId:int}/{toCurrencyId:int}")]
        public async Task<ActionResult> GetExchangeRate(int fromCurrencyId, int toCurrencyId)
        {
            var customerShipTos = await _currencyService.GetExchangeRate(fromCurrencyId, toCurrencyId);
            return Ok(customerShipTos);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateAllBasedCurrencyExchangeFixerIO()
        {
            await _currencyService.UpdateAllBasedCurrencyExchangeFixerIO();
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
