using System.Threading.Tasks;

namespace CampaignQueueMonitor
{
    public interface IOtherDesk : IDesk
    {
        void SetProperties(int serverId);
    }
}