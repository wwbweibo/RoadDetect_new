using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public class ServiceViewModel
    {
        public string ServiceId { get; set; }
        public ServiceType ServiceType { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceName { get; set; }
    }
}
