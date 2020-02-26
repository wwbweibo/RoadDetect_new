using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class CrackCalcMessageHandler:BaseMessageHandler
    {
        public CrackCalcMessageHandler(CrackDbContext dbContext) : base(dbContext)
        {

        }

        public override void HandelMessage(object sender, string message)
        {
            TaskModel taskModel = new TaskModel();
            taskModel.MergeFrom(message.DecodeBase64String());
            // todo: 逻辑待定
        }
    }
}
