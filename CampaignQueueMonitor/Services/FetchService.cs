using System.Net;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public class FetchService : IFetchService
    {
        public async Task<string> FetchCount(string metric, string url, string token)
        {
            using var client = new WebClient();
            if (!string.IsNullOrEmpty(token))
                client.Headers.Add("Authorization", token);
            var result = await client.DownloadStringTaskAsync(url);
            return result;
        }
    }
}
