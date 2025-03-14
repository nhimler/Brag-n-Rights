using GymBro_App.Models;
using GymBro_App.Services;
using GymBro_App.DAL.Abstract;
using GymBro_App.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GymBro_App.Tests.Services
{
    [TestFixture]
    public class OAuthServiceTests
    {
        private OAuthService _oAuthService;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<GymBroDbContext> _contextMock;
        private EncryptionHelper _encryptionHelper;

        [SetUp]
        public void Setup()
        {
            // Mock IHttpClientFactory, IConfiguration, IHttpContextAccessor, IUserRepository, and GymBroDbContext
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _contextMock = new Mock<GymBroDbContext>();

            // Mock the "EncryptionKey" configuration value
            _configurationMock.Setup(config => config["EncryptionKey"]).Returns(Convert.ToBase64String(new byte[32])); // 32-byte dummy key

            // Create a real instance of EncryptionHelper with the mocked configuration
            _encryptionHelper = new EncryptionHelper(_configurationMock.Object);

            // Initialize the OAuthService with the mocks and the real EncryptionHelper instance
            _oAuthService = new OAuthService(
                _httpClientFactoryMock.Object,
                _configurationMock.Object,
                _httpContextAccessorMock.Object,
                _userRepositoryMock.Object,
                _contextMock.Object,
                _encryptionHelper
            );
        }

        [Test]
        public void GenerateCodeVerifier_ShouldReturnValidBase64String()
        {
            // Act
            var codeVerifier = _oAuthService.GenerateCodeVerifier();

            // Assert
            Assert.IsNotNull(codeVerifier);
            Assert.AreEqual(86, codeVerifier.Length); // 64 bytes in Base64 URL format = 86 characters
            Assert.That(codeVerifier, Does.Not.Contain("+").And.Not.Contain("/").And.Not.Contain("="));
        }

        [Test]
        public void GenerateCodeChallenge_ShouldReturnCorrectFormat()
        {
            // Arrange
            var codeVerifier = _oAuthService.GenerateCodeVerifier();

            // Act
            var codeChallenge = _oAuthService.GenerateCodeChallenge(codeVerifier);

            // Assert
            Assert.IsNotNull(codeChallenge);
            Assert.AreEqual(43, codeChallenge.Length); // SHA256 hash of 64 bytes in Base64 URL format = 43 characters
            Assert.That(codeChallenge, Does.Not.Contain("+").And.Not.Contain("/").And.Not.Contain("="));
        }

        [Test]
        public void GenerateCodeChallenge_ShouldBeConsistent()
        {
            // Arrange
            var codeVerifier = "testVerifier";

            // Act
            var codeChallenge1 = _oAuthService.GenerateCodeChallenge(codeVerifier);
            var codeChallenge2 = _oAuthService.GenerateCodeChallenge(codeVerifier);

            // Assert
            Assert.AreEqual(codeChallenge1, codeChallenge2); // Should be deterministic
        }

        [Test]
        public void GenerateState_ShouldReturnValidBase64String()
        {
            // Act
            var state = _oAuthService.GenerateState();

            // Assert
            Assert.IsNotNull(state);
            Assert.AreEqual(43, state.Length); // 32 bytes in Base64 URL format = 43 characters
            Assert.That(state, Does.Not.Contain("+").And.Not.Contain("/").And.Not.Contain("="));
        }
    }
}
