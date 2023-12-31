using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigSharp.Test
{
    [TestClass]
    public class ConfigBagParseReferenceTest
    {
        [TestMethod]
        [TestCategory("ConfigSharp")]
        public void ParseCommandline()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(3, ConfigBag.ParseReference(@"a b c").Count);
            Assert.AreEqual(3, ConfigBag.ParseReference(@"a ""b"" c").Count);
            Assert.AreEqual(3, ConfigBag.ParseReference(@"a ""b c"" d").Count);
            Assert.AreEqual(3, ConfigBag.ParseReference("a \"b c\" d").Count);
            Assert.AreEqual(3, ConfigBag.ParseReference(@"a ""b c"" ""d e""").Count);

            // A real case
            Assert.AreEqual(2, ConfigBag.ParseReference(@"//reference ""System.Uri, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089""").Count);

            // No support for \ escaping " in "", because \ is needed in file system paths
            Assert.AreEqual(3, ConfigBag.ParseReference(@"a ""b c\"" d").Count);
        }
    }
}
