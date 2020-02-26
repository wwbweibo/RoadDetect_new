using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Libs.MySql;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public abstract class BaseMessageHandler
    {
        protected CrackDbContext dbContext;
        public BaseMessageHandler(CrackDbContext dbContext)
        {
            this.dbContext = dbContext; 
        }
        public abstract void HandelMessage(object sender, string message);
    }
}
