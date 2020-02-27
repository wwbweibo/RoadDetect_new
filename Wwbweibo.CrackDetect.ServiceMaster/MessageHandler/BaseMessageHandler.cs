using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Redis;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public abstract class BaseMessageHandler
    {
        protected CrackDbContext dbContext;
        protected IRedisClient redisClient = Program.GetRedisClient();
        public BaseMessageHandler(CrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public abstract void HandelMessage(object sender, string message);
    }
}
