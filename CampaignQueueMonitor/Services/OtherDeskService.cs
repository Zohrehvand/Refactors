using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public class OtherDeskService : IOtherDesk
    {
        private readonly IFetchService _fetchService;
        private readonly ILogger<OtherDeskService> _logger;
        private readonly ISendDataService _sendDataService;

        private string _url { get; set; }
        public string Metric { get; set; }

        public OtherDeskService(ISendDataService sendDataService, ILogger<OtherDeskService> logger, IFetchService fetchService)
        {
            _fetchService = fetchService;
            _logger = logger;
            _sendDataService = sendDataService;
        }

        public void SetProperties(int serverId)
        {
            _url = $"http://{serverId}.localhost.com/count";
            Metric = $"Campaign.{serverId}";
        }

        public async Task SendData()
        {
            var content = await _fetchService.FetchCount(Metric, _url, string.Empty);
            var count = ExtractCount(content);
            _logger.LogInformation($"Server: {Metric} Campaign Queue Size: {count}");
            await _sendDataService.SendData(Metric, count);
        }

        private string ExtractCount(string content)
        {
            var newCount = "new count: (.*)";
            var match = new Regex(newCount, RegexOptions.IgnoreCase).Match(content);
            var campaignCount = match.Groups[1].Value;
            return campaignCount;
        }
    }
}
