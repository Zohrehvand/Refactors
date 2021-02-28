using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public interface IDesk
    {
        string Metric { get; set; }
        Task SendData();
    }
}