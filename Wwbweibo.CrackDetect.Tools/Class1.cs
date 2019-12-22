using System;

namespace Wwbweibo.CrackDetect.Tools.String
{
    public static class StringExtentions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
        }
    }
}
