using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void TicksOfSecond()
        {
            Assert.AreEqual(8_979_323, new DateTime(314_159_265_358_979_323).TicksOfSecond());
        }

    }
}
