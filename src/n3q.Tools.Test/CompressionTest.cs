using System.Security.Cryptography;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class CompressionTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void GzipCompressDecompress()
        {
            Assert.AreEqual("Hello World", Compression.GZipDecompressToString(Compression.GZipCompress("Hello World")));

            // Arrange
            var json = "{\"poi54jskv9obc8jhhezm5v5j7l6z14meghr\":{\"Checksum\":\"NjT+Xm93k/+/sqkaZ59cI5n0e9lqPQxG+GznOMlKqow=\",\"Height\":\"80\",\"Id\":\"poi54jskv9obc8jhhezm5v5j7l6z14meghr\",\"IframeAspect\":\"true\",\"IframeFrame\":\"Popup\",\"IframeHeight\":\"200\",\"IframeResizeable\":\"true\",\"IframeUrl\":\"http://localhost:5000/ItemFrame/Points?context={context}\",\"IframeWidth\":\"300\",\"ImageUrl\":\"http://localhost:5000/images/Items/User/Points.png\",\"InventoryX\":\"300\",\"InventoryY\":\"88\",\"IsRezzed\":\"true\",\"Label\":\"Points\",\"OwnerName\":\"Heiner\",\"PackedAes128\":\"SWzX7ZpX6MHwPsO8WmINa/hZ2z7EeveW/CsKqjwjBBIOuir15bRsqFVwfzO2gTt9\",\"PointsAspect\":\"true\",\"PointsChannelChat\":\"0\",\"PointsChannelEmote\":\"0\",\"PointsChannelPageOwned\":\"0\",\"PointsChannelGreet\":\"1\",\"PointsChannelItemApply\":\"0\",\"PointsChannelItemRez\":\"0\",\"PointsChannelNavigation\":\"0\",\"PointsCurrent\":\"12116420\",\"PointsTotal\":\"13462690\",\"Provider\":\"nine3q\",\"RezzedDestination\":\"https://www.galactic-developments.de/data2.html?lang=de&hilite=3131\",\"RezzedLocation\":\"d954c536629c2d729c65630963af57c119e24836@muc4.virtual-presence.org\",\"RezzedX\":\"663\",\"Stats\":\"PointsTotal PointsCurrent\",\"Template\":\"Points\",\"TransferableAspect\":\"false\",\"Width\":\"80\",\"OwnerId\":\"midv88tsoajdlagmx4qnbd5ooqrvjsis1\"}}";
            // Act
            var conpressedJson = Compression.GZipCompress(json);
            // Assert
            Assert.AreEqual(json, Compression.GZipDecompressToString(conpressedJson));

            // Arrange
            var sb = new StringBuilder();
            for (var i = 0; i < 100000; i++) { sb.Append(i); }
            var bulk = sb.ToString();
            // Act
            var compressedBulk = Compression.GZipCompress(bulk);
            // Assert
            Assert.AreEqual(bulk, Compression.GZipDecompressToString(compressedBulk));
        }
    }
}
