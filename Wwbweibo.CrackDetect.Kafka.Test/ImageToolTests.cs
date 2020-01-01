using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wwbweibo.CrackDetect.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using OpenCvSharp;
using Wwbweibo.CrackDetect.Tools;

namespace Wwbweibo.CrackDetect.Tools.Tests
{
    [TestClass()]
    public class ImageToolTests
    {
        [TestMethod()]
        public void DecodeByteImageTest()
        {
            var path = "test.jpg";
            var file = File.ReadAllBytes(path);
            var mat = ImageTool.DecodeByteImage(file);

            Assert.IsNotNull(mat);
        }

        [TestMethod()]
        public void EncodeImageTest()
        {
            var mat = new Mat("test.jpg", ImreadModes.AnyColor);
            var data =ImageTool.EncodeImage(mat, ".jpg");
            Assert.IsTrue(data.Length >0);
        }
    }
}