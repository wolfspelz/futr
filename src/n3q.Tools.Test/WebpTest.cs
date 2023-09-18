using System;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class WebpTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void GetDurationMSec_Webp()
        {
            foreach (var (expected, name) in new []{
                new Tuple<int,string>(0, "static_lossless_alpha_200x200.webp"),
                new Tuple<int,string>(966, "animation_lossy_alpha_200x200_966ms.webp"),
                new Tuple<int,string>(1833, "animation_lossy_alpha_200x200_1833ms.webp"),
            }) {
                var data = GetMediaData(name);
                var duration = Webp.GetDurationMSec(data);
                Assert.AreEqual(expected, duration, $"Wrong duration for {name}!");
            }
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void GetDurationMSec_NotWebp()
        {
            foreach (var name in new []{
                "indexed_alpha_100x100.png",
                "static_indexed_alpha_100x100.gif",
                "animation_indexed_alpha_100x100.gif",
            }) {
                var data = GetMediaData(name);
                Action fun = () => Webp.GetDurationMSec(data);
                Assert.ThrowsException<SyntaxErrorException>(fun, $"Wrong duration for {name}!");
            }
        }

        protected byte[] GetMediaData(string name) {
            var cwd = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(cwd, "../../../data/Media/", name);
            return File.ReadAllBytes(filePath);
        }
    }
}
