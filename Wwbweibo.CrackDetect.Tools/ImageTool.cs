using System.Globalization;
using OpenCvSharp;

namespace Wwbweibo.CrackDetect.Tools
{
    public class ImageTool
    {
        /// <summary>
        /// decode the byte data
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Mat DecodeByteImage(byte[] bytes)
        {
            return Mat.FromImageData(bytes);
        }

        /// <summary>
        /// encode a mat to image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ext"></param>
        public static byte[] EncodeImage(Mat image, string ext)
        {
            return image.ImEncode(ext);
        }
    }
}
