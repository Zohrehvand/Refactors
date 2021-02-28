using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public class ZenDeskService : IZenDesk
    {
        private readonly IFetchService _fetchService;
        private readonly ILogger<ZenDeskService> _logger;
        private readonly ISendDataService _sendDataService;

        private readonly string _url;
        private readonly string _token;
        public string Metric { get; set; }

        public ZenDeskService(ISendDataService sendDataService, ILogger<ZenDeskService> logger, IFetchService fetchService, IConfiguration configuration)
        {
            _url = configuration.GetValue<string>("ZenDesk:Url");
            _token = configuration.GetValue<string>("ZenDesk:Token");
            Metric = configuration.GetValue<string>("ZenDesk:Metric");
            _fetchService = fetchService;
            _logger = logger;
            _sendDataService = sendDataService;
        }

        public async Task SendData()
        {
            var content = await _fetchService.FetchCount(Metric, _url, _token);
            var count = ExtractCount(content);
            _logger.LogInformation($"Zendesk Engineering Ticket count: {count}");
            await _sendDataService.SendData(Metric, count);
        }
        private string ExtractCount(string content)
        {
            return JObject.Parse(content)["count"].ToString();
        }
    }
}
