using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Wwbweibo.CrackDetect.Models;

namespace Wwbweibo.CrackDetect.ServiceMaster.Models
{
    public static class ConstData
    {
        public static string[] TaskTypes = new[] {"preprocess", "crackcalc"};
        public static string PreProcessTaskName = "preprocess";
        public static string CrackCalcTaskName = "crackcalc";
    }
}
