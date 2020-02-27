namespace Wwbweibo.CrackDetect.Libs.Redis
{
    public interface IRedisClient
    {
        void Set(string key, string value);
        string Get(string key);

        string[] LPop(string key, int start = -1, int end = -1);

        void LPush(string key, string[] values);

        void HSet(string key, string name, string value);
        string HGet(string key, string name);
    }
}
