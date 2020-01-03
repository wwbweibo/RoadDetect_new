using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Wwbweibo.CrackDetect.Tools.String;

namespace Wwbweibo.CrackDetect.Tools
{
    public static class Logger
    {
        private static string baseLogFileName = "log-{0}-{1}.log";
        public static void Error(DateTime time, string message,Exception e)
        {
            var logfilename = baseLogFileName.Format("error", DateTime.Now.ToShortDateString());
            using (var sw = new StreamWriter(new FileStream(logfilename, FileMode.OpenOrCreate)))
            {
                
                var errorMessage = "{0}\n\r{1}\n\r{2}\n\r{3}\n\r\n\r";
                errorMessage = errorMessage.Format(errorMessage, time.ToLongTimeString(), message,e.Message, e.StackTrace);
                sw.WriteLineAsync(errorMessage);
            }
        }

        public static void Info(DateTime time, string message, StackTrace trace = null)
        {
            var logfilename = baseLogFileName.Format("info", DateTime.Now.ToShortDateString());
            using (var sw = new StreamWriter(new FileStream(logfilename, FileMode.OpenOrCreate)))
            {
                var errorMessage = "{0}\n\r{1}\n\r{2}\n\r\n\r";
                errorMessage = errorMessage.Format(errorMessage, time.ToLongTimeString(), message, trace?.ToString());
                sw.WriteLineAsync(errorMessage);
            }
        }
    }
}
