using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SERP.Api.Common;
using SERP.Application.Masters.ApplicationTokens.Services;
using SERP.Application.Transactions.ApplicationToken.Response;

namespace SERP.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationTokenController : ControllerBase
    {
        private readonly IMapper _mapper;
        private HttpContextService _httpContextService;
        private readonly IApplicationTokenService _applicationTokenService;
        
        public ApplicationTokenController(IMapper mapper, IApplicationTokenService applicationTokenService)
        {
            _mapper = mapper;
            _applicationTokenService = applicationTokenService;
            _httpContextService = new HttpContextService();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]/{applicationCode}")]
        public async Task<object> GetByApplicationCode(string applicationCode)
        {
            ApplicationTokenDto token = await _applicationTokenService.GetByApplicationCode(_httpContextService.GetCurrentUserId(), applicationCode);
            return Ok(token);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("[action]")]
        public async Task CreateApplicationToken()
        {
            await _applicationTokenService.CreateTokenAsync(_httpContextService.GetCurrentUserId());
        }
    }
}
