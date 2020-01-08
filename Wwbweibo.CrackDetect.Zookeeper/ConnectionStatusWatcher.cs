using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNetEx;
using org.apache.zookeeper;

namespace Wwbweibo.CrackDetect.Zookeeper
{
    public class ConnectionStatusWatcher:Watcher
    {
        private static ZookeeperClient zkClient;
        public ConnectionStatusWatcher(ZookeeperClient client)
        {
            zkClient = client;
        }
        public override Task process(WatchedEvent @event)
        {
            if (@event.getState() == Event.KeeperState.Disconnected  ||@event.getState() == Event.KeeperState.Expired)
            {
                zkClient.InitClientConnection();
            }
            
            return null;
        }
    }
}
