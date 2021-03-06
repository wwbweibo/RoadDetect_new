﻿using Google.Protobuf;
using System;
using System.Linq;
using Wwbweibo.CrackDetect.Libs.Tools.String;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.MessageHandler
{
    public class CrackCalcMessageHandler : BaseMessageHandler
    {

        public override void HandelMessage(object sender, string message)
        {
            TaskResultModel taskModel = new TaskResultModel();
            taskModel.MergeFrom(message.DecodeBase64String());
            if (!taskModel.MajorTaskId.IsNullOrEmpty() && !taskModel.SubTaskId.IsNullOrEmpty())
            {
                lock (dbContext.obj)
                {
                    var item = dbContext.TaskItems.FirstOrDefault(p => p.Id == Guid.Parse(taskModel.SubTaskId));
                    if (item != null)
                    {
                        item.Data = redisClient.HGet(taskModel.MajorTaskId, taskModel.SubTaskId);
                        item.IsCrack = taskModel.IsCrack;
                        item.Area = taskModel.CrackArea;
                        if(taskModel.IsCrack)
                            item.MarkedData = redisClient.HGet(taskModel.MajorTaskId, "result-" + taskModel.SubTaskId);
                    }
                    // 接收到该信息表明该任务已经被完成了,删除对应的缓存
                    redisClient.HDelete(taskModel.MajorTaskId, taskModel.SubTaskId);
                    if(taskModel.IsCrack) 
                        redisClient.HDelete(taskModel.MajorTaskId, "result-" + taskModel.SubTaskId);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}