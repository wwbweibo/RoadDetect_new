using System;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public class TaskItemViewModel
    {
        public Guid TaskItemId { get; set; }
        public Guid MajorTaskId { get; set; }
        public string OriginImageData { get; set; }
        public string MarkedImageData { get; set; }
        public double Area { get; set; }
        public bool IsCrack { get; set; }
        public Position Position { get; set; }
    }
}