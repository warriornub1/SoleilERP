using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.Customers.Services;

namespace SERP.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private HttpContextService _httpContextService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
            _httpContextService = new HttpContextService();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllLimited(bool onlyEnabled)
        {
            var customers = await _customerService.GetAllLimited(onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, customers);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{customerId:int}/{onlyEnabled:bool}")]
        public async Task<ActionResult> GetAllShipToByCustomer(int customerId, bool onlyEnabled)
        {
            var customerShipTos = await _customerService.GetAllShipToByCustomer(customerId, onlyEnabled);
            return StatusCode(StatusCodes.Status200OK, customerShipTos);
        }
    }
}
