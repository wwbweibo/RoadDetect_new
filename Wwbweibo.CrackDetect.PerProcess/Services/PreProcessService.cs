using System.Collections.Generic;
using OpenCvSharp;
using Wwbweibo.CrackDetect.Tools;
using Wwbweibo.CrackDetect.Tools.String;

namespace Wwbweibo.CrackDetect.PerProcess.Services
{
    public class PreProcessService
    {
        private Mat image;
        private Dictionary<int, Mat> cutImageDic;
        public PreProcessService(Mat image)
        {

            this.image = image;
        }

        public PreProcessService(string base64)
        {
            this.image = ImageTool.DecodeByteImage(base64.DecodeBase64String());
        }

        public void ExecuteWorkFlow()
        {

        }

        /// <summary>
        /// 提升图像对比度
        /// </summary>
        private void IncreaseContrast()
        {

        }

        private Dictionary<int, Mat> CutImage()
        {
            for (var i = 0; i < 64; i++)
            {
                for (var j = 0; j < 64; j++)
                {
                    cutImageDic.Add(i * 16 + 1, image.AdjustROI(16 * i, 16 * (i + 1), 16 * j, 16 * (j + 1)));
                }
            }

            return cutImageDic;
        }
    }
}

