#nullable disable
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class IsTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void Value()
        {
            Assert.IsTrue(Is.Value("42"));
            Assert.IsFalse(Is.Value(""));
            Assert.IsFalse(Is.Value(null));

            string a = null; Assert.IsFalse(Is.Value(a));
            string b = ""; Assert.IsFalse(Is.Value(b));
            string c = "42"; Assert.IsTrue(Is.Value(c));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void Thing()
        {
            Assert.IsTrue(Is.Thing("a"));
            Assert.IsTrue(Is.Thing(""));
            Assert.IsFalse(Is.Thing(null));
            var o = new object(); Assert.IsTrue(Is.Thing(o));
        }

    }
}
