using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public static class ConstData
    {
        public static string[] TaskTypes = new[] {"preprocess", "crackcalc"};
        public static string PreProcessTaskName = "preprocess";
        public static string CrackCalcTaskName = "crackcalc";

        public static string PythonPreprocessServiceName = "python-preprocess-service";
        public static string ServiceMasterServiceName = "service-master-service";
        public static string[] ServiceTypes = new[] { PythonPreprocessServiceName, ServiceMasterServiceName };

    }
}
