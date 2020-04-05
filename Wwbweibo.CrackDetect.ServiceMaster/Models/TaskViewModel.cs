using System;
using System.Collections.Generic;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public class TaskViewModel
    {
        public Guid MajorTaskId { get; set; }
        public List<TaskItemViewModel> TaskItemList { get; set; }
        public Position Position { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ImageData { get; set; }
        public int TotalCount { get; set; }
        public int CrackCount { get; set; }
    }
}