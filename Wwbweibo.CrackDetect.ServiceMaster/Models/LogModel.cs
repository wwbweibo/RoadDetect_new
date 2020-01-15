using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public class LogModel
    {
        public string LogTime { get; set; }
        public string LogLevel { get; set; }
        public string LogMessage { get; set; }
        public string OriginServiceType { get; set; }
        public string OriginServiceId { get; set; }
        public string Exception { get; set; }
    }
}
