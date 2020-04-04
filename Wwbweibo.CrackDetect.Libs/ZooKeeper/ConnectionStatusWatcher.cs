using org.apache.zookeeper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Wwbweibo.CrackDetect.Libs.Zookeeper
{
    public class ConnectionStatusWatcher : Watcher
    {
        private static ZookeeperClient zkClient;
        public ConnectionStatusWatcher(ZookeeperClient client)
        {
            zkClient = client;
        }
        public override Task process(WatchedEvent @event)
        {
            if (@event != null)
            {
                if (@event.getState() == Event.KeeperState.Disconnected || @event.getState() == Event.KeeperState.Expired)
                {
                   return new Task(() => zkClient.InitClientConnection()); 
                }
            }
            return new Task(() => {
            });
        }
    }
}
