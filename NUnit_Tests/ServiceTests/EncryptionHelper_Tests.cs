using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using GymBro_App.Helper;
using System;

namespace GymBro_App.Tests.Helper
{
    [TestFixture]
    public class EncryptionHelperTests
    {
        private EncryptionHelper _encryptionHelper;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"EncryptionKey", Convert.ToBase64String(new byte[32] // 32 bytes key
                {
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10,
                    0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
                    0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20
                })}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _encryptionHelper = new EncryptionHelper(configuration);
        }

        [Test]
        public void EncryptToken_ValidToken_ReturnsEncryptedString()
        {
            // Arrange
            string token = "TestToken123";

            // Act
            string encryptedToken = _encryptionHelper.EncryptToken(token);

            // Assert
            Assert.IsNotNull(encryptedToken);
            StringAssert.Contains(":", encryptedToken); // Should be in IV:Data format
        }

        [Test]
        public void DecryptToken_ValidEncryptedString_ReturnsOriginalToken()
        {
            // Arrange
            string originalToken = "TestToken123";
            string encryptedToken = _encryptionHelper.EncryptToken(originalToken);

            // Act
            string decryptedToken = _encryptionHelper.DecryptToken(encryptedToken);

            // Assert
            Assert.AreEqual(originalToken, decryptedToken);
        }

        [Test]
        public void DecryptToken_InvalidFormat_ReturnsNull()
        {
            // Act
            string result = _encryptionHelper.DecryptToken("InvalidFormatString");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Constructor_MissingKey_ThrowsException()
        {
            // Arrange
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>()).Build();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => new EncryptionHelper(config));
            Assert.AreEqual("Encryption key is missing.", ex.Message);
        }

        [Test]
        public void Constructor_InvalidKeyLength_ThrowsException()
        {
            var invalidKey = Convert.ToBase64String(new byte[16]); // Wrong length

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "EncryptionKey", invalidKey }
                })
                .Build();

            var ex = Assert.Throws<Exception>(() => new EncryptionHelper(config));
            Assert.AreEqual("Encryption key must be exactly 32 bytes.", ex.Message);
        }
    }
}
