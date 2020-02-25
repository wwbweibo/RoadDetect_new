﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Crmf;
using Wwbweibo.CrackDetect.Libs.MySql;
using Wwbweibo.CrackDetect.Libs.MySql.Entity;
using Wwbweibo.CrackDetect.Models;
using DbTask = Wwbweibo.CrackDetect.Libs.MySql.Entity.Task;

namespace Wwbweibo.CrackDetect.ServiceMaster.Services
{
    public class TaskService
    {
        private readonly CrackDbContext dbContext;

        public async Task<bool> CreateTask(TaskModel taskModel)
        {
            var task = new DbTask(){Id = Guid.Parse(taskModel.MajorTaskId), StartTime = DateTime.Parse(taskModel.TaksStartTime)};
            return await dbContext.CreateTask(task);
        }
    }
}