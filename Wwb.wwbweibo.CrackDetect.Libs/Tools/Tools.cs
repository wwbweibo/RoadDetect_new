using Newtonsoft.Json;
using System;

namespace Wwbweibo.CrackDetect.Libs.Tools
{
    public static class Tools
    {
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds * 1000);
        }

        public static string Parse2Json(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ParseJson2Object<T>(string s) where T : class
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

    }
}
