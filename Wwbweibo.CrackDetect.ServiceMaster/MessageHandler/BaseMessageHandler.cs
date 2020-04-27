using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Redis;
using Wwbweibo.CrackDetect.Libs.Zookeeper;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public abstract class BaseMessageHandler
    {
        public CrackDbContext dbContext { get; set; }
        public IRedisClient redisClient { get; set; }
        public ZookeeperClient ZookeeperClient { get; set; }

        public abstract void HandelMessage(object sender, string message);
    }
}
