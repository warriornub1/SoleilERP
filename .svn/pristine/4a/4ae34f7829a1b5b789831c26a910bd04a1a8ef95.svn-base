using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SERP.Application.Transactions.Test;
using SERP.Domain.Transactions.PurchaseOrders;
using static System.Net.Mime.MediaTypeNames;

namespace SERP.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPODetailsTest()
        {
            (string inCache, var result) = await _testService.TestGetAllPODetail();

            return Ok(new PODetailTest
            {
                inCache = inCache,
                result = result,
            });
        }

        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background Job Triggered"));

            BackgroundJob.Enqueue<TestJob>(x => x.print(0));
            return Ok();
        }

        [HttpPost]
        [Route("CreateBackgroundJobForLoop")]
        public ActionResult CreateBackgroundJobForLoop()
        {
            Console.WriteLine("Starting Background Job");
            for (int i = 0; i < 100; i++)
            {
                int capturedI = i; // Capture the current value of i
                BackgroundJob.Enqueue<TestJob>(x => x.print(capturedI));

            }
            Console.WriteLine("Ending Background Job");
            return Ok();
        }
    }


    public class PODetailTest
    {
        public string inCache { get; set; }
        public IEnumerable<PODetail> result { get; set; }
    }
}