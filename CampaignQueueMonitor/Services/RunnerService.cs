using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public class RunnerService : IRunnerService
    {
        private readonly ILogger<RunnerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RunnerService(ILogger<RunnerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Run()
        {
            var desks = CreateDeskList();
            await SendAllData(desks);
        }

        private List<IDesk> CreateDeskList()
        {
            short[] serverIds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var desks = new List<IDesk>();

            foreach (var serverId in serverIds)
            {
                var otherDesk = _serviceProvider.GetService<IOtherDesk>();
                otherDesk.SetProperties(serverId);
                desks.Add(otherDesk);
            }
            var zenDesk = _serviceProvider.GetService<IZenDesk>();
            desks.Add(zenDesk);
            return desks;
        }

        private async Task SendAllData(List<IDesk> desks)
        {
            foreach (var desk in desks)
            {
                try
                {
                    await desk.SendData();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"We had problem to send data to the server {desk.Metric}: {ex.Message}");
                }
            }
        }
    }
}
