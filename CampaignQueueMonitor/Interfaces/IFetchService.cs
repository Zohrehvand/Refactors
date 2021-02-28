using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public interface IFetchService
    {
        Task<string> FetchCount(string metric, string url, string token);
    }
}
