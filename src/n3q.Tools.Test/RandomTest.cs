using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void RandomString_are_different()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsTrue(RandomString.Alphanum(20) != RandomString.Alphanum(20));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void RandomString_desired_legth()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(10, RandomString.Alphanum(10).Length);
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void NextTriangular()
        {
            // Arrange
            var r = new Random(1);
            var n = 100000;
            var res = new double[n];
            var min = 2;
            var peak = 6;
            var max = 8;
            for (var i = 0; i < n; i++) {
                // Act
                res[i] = r.NextTriangular(min, peak, max);
                //res[i] = r.NextTriangular();
            }

            // Assert
            for (var i = 0; i < n; i++) {
                var x = res[i];
                Assert.IsTrue(x >= min && x <= max);
            }

            Assert.ThrowsException<ArgumentException>(() => r.NextTriangular(1, 3, 2));
            Assert.ThrowsException<ArgumentException>(() => r.NextTriangular(3, 2, 1));
            Assert.AreEqual(2, r.NextTriangular(2, 2, 2));

            //File.WriteAllLines(@"C:\Users\wolf\AppData\Local\Temp\x.txt", res.Select(x => x.ToString()));
        }

        [TestMethod]
        [TestCategory("Tools")]
        public void NextGaussian()
        {
            // Arrange
            var r = new Random(1);
            var n = 100000;
            var res = new double[n];
            for (var i = 0; i < n; i++) {
                // Act
                res[i] = r.NextGaussian();
            }

            // Assert
            //File.WriteAllLines(@"C:\Users\wolf\AppData\Local\Temp\x.txt", res.Select(x => x.ToString()));
        }

    }
}
