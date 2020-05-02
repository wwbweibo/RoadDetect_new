using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wwbweibo.CrackDetect.ServiceMaster.Utils
{
    public static class BaiduMapUtils
    {
        private static string ak = "99f2670196ca0b8c0cfbe7f5139ad3a9";

        public static async Task<string> LocationConvert(double lat, double lng)
        {

            var url =
                $"https://restapi.amap.com/v3/geocode/regeo?location={lat},{lng}&key={ak}&extensions=all";
            using (HttpClient client = new HttpClient())
            {
                var data = await client.GetStringAsync(url);
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                var location =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(obj["regeocode"].ToString())[
                        "formatted_address"].ToString();
                var road =
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        (JsonConvert.DeserializeObject<Dictionary<string, object>>(obj["regeocode"].ToString())["roads"]
                            as
                            IEnumerable<object>).First().ToString())["name"].ToString();
                var ret = road + " " + location + "附近";
                return ret;
            }
        }
    }
}