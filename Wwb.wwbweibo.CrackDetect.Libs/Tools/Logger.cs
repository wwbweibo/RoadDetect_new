using System;
using System.Diagnostics;
using System.IO;
using Wwbweibo.CrackDetect.Libs.Tools.String;

namespace Wwbweibo.CrackDetect.Libs.Tools
{
    public static class Logger
    {
        private static string baseLogFileName = "log-{0}-{1}.log";
        public static void Error(string message, Exception e)
        {
            var time = DateTime.Now;
            var logfilename = baseLogFileName.Format("error", DateTime.Now.ToShortDateString());
            using (var sw = new StreamWriter(new FileStream(logfilename, FileMode.OpenOrCreate)))
            {

                var errorMessage = "{0}\n\r{1}\n\r{2}\n\r{3}\n\r\n\r";
                errorMessage = errorMessage.Format(errorMessage, time.ToLongTimeString(), message, e.Message, e.StackTrace);
                sw.WriteLineAsync(errorMessage);
            }
        }

        public static void Info(string message, StackTrace trace = null)
        {
            var time = DateTime.Now;
            var logfilename = baseLogFileName.Format("info", DateTime.Now.ToShortDateString());
            using (var sw = new StreamWriter(new FileStream(logfilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)))
            {
                var errorMessage = "{0}\n\r{1}\n\r{2}\n\r\n\r";
                errorMessage = errorMessage.Format(errorMessage, time.ToLongTimeString(), message, trace?.ToString());
                sw.WriteLineAsync(errorMessage);
            }
        }
    }
}
