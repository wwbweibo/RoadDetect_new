using Google.Protobuf;
using System;
using System.IO;

namespace Wwbweibo.CrackDetect.Libs.Tools.String
{
    public static class StringExtensions
    {
        /// <summary>
        /// check the given string is null or empty
        /// </summary>
        /// <param name="s">input string</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) && string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// decode from a base64 encoded string to byte array
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] DecodeBase64String(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// convert a byte array to base64 encoded string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeBytesToBase64String(this byte[] data)
        {
            if (data == null)
                throw new ApplicationException("the input data can not be null");
            return Convert.ToBase64String(data);
        }

        public static string Format(this string s, params string[] args)
        {
            try
            {
                return string.Format(s, args);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        public static string ToB64Data(this IMessage message)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                message.WriteTo(ms);
                return ms.ToArray().EncodeBytesToBase64String();
            }
        }
    }
}
