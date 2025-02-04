using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.Application.Common.BackendSchedulerTask.Services
{
    public interface ICronJobService
    {
        Task StartAsync(CancellationToken cancellationToken); 
        Task StopAsync(CancellationToken cancellationToken);
        Task DoWork();

    }
}
