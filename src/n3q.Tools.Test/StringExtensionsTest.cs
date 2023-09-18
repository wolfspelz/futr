#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void IsSomething()
        {
            Assert.IsTrue(Is.Value("x"));
            Assert.IsFalse(Is.Value(""));
            Assert.IsFalse(Is.Value((string)null));
            Assert.IsFalse(Is.Value(null));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void SimpleHash_spreads()
        {
            var histogram = new Dictionary<int, int>();
            var len = 2;
            var mod = (int)Math.Pow(10, len);
            var count = 100000;
            var average = count / mod;
            for (var i = 0; i < mod; i++) {
                histogram[i] = 0;
            }
            for (var i = 0; i < count; i++) {
                var s = RandomString.Alphanum(RandomInt.Between(1, 30));
                //var s = Guid.NewGuid().ToString();
                var hash = s.SimpleHash();
                var sHash2 = hash % mod;
                histogram[sHash2]++;
            }
            var values = histogram.Values.OrderBy(x => x).ToList();
            var inverted = histogram.Select(pair => new KeyValuePair<int, int>(pair.Value, pair.Key)).OrderBy(kv => kv.Key);
            for (var i = 0; i < mod; i++) {
                Assert.IsTrue(histogram[i] > average * 0.8);
                Assert.IsTrue(histogram[i] < average * 1.2);
            }
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void XmlEncode()
        {
            Assert.AreEqual("abc123ü&amp;&lt;&gt;\"'\n\r\t", "abc123ü&<>\"'\n\r\t".XmlEncode());
            Assert.AreEqual("abc123ü&amp;&lt;&gt;&quot;&apos;&#xA;&#xD;&#x9;", "abc123ü&<>\"'\n\r\t".XmlEncode(isAttribute: true));
        }

    }
}
