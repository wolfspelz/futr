using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class CrcTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void ToDouble_CheckEqualDistribution()
        {
            // Arrange
            var n = 1000000;
            var slots = 100;
            var hist = new int[slots];
            for (int i = 0; i < slots; i++) {
                hist[i] = 0;
            }

            for (int i = 0; i < n; i++) {
                var hash = Crypto.SHA1Hex(i.ToString());

                // Act
                var floatNum = Crc32.ToDouble(hash);
                hist[(int)Math.Floor(floatNum * slots)]++;
            }

            // Assert
            var perSlot = n / slots;
            var sig = perSlot / Math.Sqrt(perSlot);
            var delta = 3 * sig;
            var min = perSlot - delta;
            var max = perSlot + delta;
            for (int i = 0; i < slots; i++) {
                Assert.IsTrue(hist[i] > min, $"{i}: {hist[i]} not between {min} {max}");
                Assert.IsTrue(hist[i] < max, $"{i}: {hist[i]} not between {min} {max}");
            }

            //File.WriteAllLines(@"C:\Users\wolf\AppData\Local\Temp\hist.txt", hist.Select(x => x.ToString()));
        }
    }
}
