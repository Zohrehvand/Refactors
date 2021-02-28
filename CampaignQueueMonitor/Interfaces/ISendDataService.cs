using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public interface ISendDataService
    {
        Task SendData(string metric, string count);
    }
}
