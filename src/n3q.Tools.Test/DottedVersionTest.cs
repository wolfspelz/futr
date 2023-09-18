using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class DottedVersionTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void Value()
        {
            Assert.IsTrue(DottedVersion.Long("1") == DottedVersion.Long("1.0"));
            Assert.IsTrue(DottedVersion.Long("1") == DottedVersion.Long("1.0.0.0"));
            Assert.IsTrue(DottedVersion.Long("1.0.1") == DottedVersion.Long("1.0.1.0"));
            Assert.IsTrue(DottedVersion.Long("1") > DottedVersion.Long("0.9"));
            Assert.IsTrue(DottedVersion.Long("1.1") >= DottedVersion.Long("1.0"));
            Assert.IsTrue(DottedVersion.Long("2") > DottedVersion.Long("1"));
            Assert.IsTrue(DottedVersion.Long("2.5.1.1845") > DottedVersion.Long("2.4.2"));
            Assert.IsTrue(DottedVersion.Long("2.5.1.1845") > DottedVersion.Long("2.5.1.1844"));
            Assert.IsTrue(DottedVersion.Long("10000.0.0.0") > DottedVersion.Long("9999.9999.9999.9999"));
            Assert.IsTrue(DottedVersion.Long("10000.0.0.0") > DottedVersion.Long("9999.99999.99999.99999"));
            Assert.IsTrue(DottedVersion.Long("10.0.17763.0") > DottedVersion.Long("10.0.17134.0"));
            Assert.IsTrue(DottedVersion.Long("10.0.17763.1") > DottedVersion.Long("10.0.17134.0"));
            Assert.IsTrue(DottedVersion.Long("10.1.1.0") > DottedVersion.Long("10.0.17134.0"));
            Assert.IsTrue(DottedVersion.Long("10.0.17134.1") > DottedVersion.Long("10.0.17134.0"));
        }

    }
}
