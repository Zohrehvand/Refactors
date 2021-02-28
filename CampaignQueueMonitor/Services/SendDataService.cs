using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public class SendDataService : ISendDataService
    {
        private readonly IConfiguration _configuration;

        public SendDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendData(string metric, string count)
        {
            var visualiserSeriesUri = _configuration.GetValue<string>("Visualiser:SeriesUri");
            var visualiserApiKey = _configuration.GetValue<string>("Visualiser:ApiKey");
            var requestUri = visualiserSeriesUri + "?api_key=" + visualiserApiKey;

            var epochTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            
            var json = "{\"series\":[{\"metric\":\"" + metric + "\",\"points\":[[" + epochTimestamp + "," + count + "]],\"type\":\"count\"}]}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var _httpClient = new HttpClient();
            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
