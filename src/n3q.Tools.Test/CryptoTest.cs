using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace n3q.Tools.Test
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        [TestCategory("Tools")]
        public void AesEncryptDecrypt()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsTrue("Hello World" == Crypto.AesDecryptToString(Crypto.AesEncrypt("Hello World", "b14ca5898a4e4133bbce2ea2315a1916"), "b14ca5898a4e4133bbce2ea2315a1916"));
            Assert.IsTrue("Hello World" == Crypto.AesDecryptToString(Crypto.AesEncrypt("Hello World", "12345678901234567890123456789012"), "12345678901234567890123456789012"));
            Assert.IsTrue("Hello World" == Crypto.AesDecryptToString(Crypto.AesEncrypt("Hello World", "42b3154909be91eaad03892cf98ec34a"), "42b3154909be91eaad03892cf98ec34a"));
            Assert.IsTrue("A" == Crypto.AesDecryptToString(Crypto.AesEncrypt("A", "42b3154909be91eaad03892cf98ec34a"), "42b3154909be91eaad03892cf98ec34a"));
            Assert.IsFalse("A" == Crypto.AesDecryptToString(Crypto.AesEncrypt("B", "42b3154909be91eaad03892cf98ec34a"), "42b3154909be91eaad03892cf98ec34a"));
            Assert.ThrowsException<CryptographicException>(() => Crypto.AesDecryptToString(Crypto.AesEncrypt("Hello World", "12345678901234567890123456789012"), "12345678901234567890123456789013"));
        }
    }
}
