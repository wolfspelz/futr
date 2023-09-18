using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class MiscTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void DateTime_MinValue_equals_0L_Utc()
        {
            Assert.IsTrue(DateTime.MinValue == new DateTime(0L, DateTimeKind.Utc));
        }
    }
}
