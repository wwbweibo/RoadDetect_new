using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wwbweibo.CrackDetect.Tools.String;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wwbweibo.CrackDetect.Tools.String.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            var s = "ag";
            Assert.IsFalse(s.IsNullOrEmpty());
            s = "";
            Assert.IsTrue(s.IsNullOrEmpty());
        }
    }
}