using Microsoft.Extensions.Logging;

namespace SERP.Application.Transactions.Test
{
    public class TestJob
    {
        public void print(int i)
        {
            Console.WriteLine($"Background Job Triggered Loop {i}");
            Thread.Sleep(2000);
        }
    }
}
