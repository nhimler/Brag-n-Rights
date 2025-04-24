using GymBro_App.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace NUnit_Tests.ServiceTests
{
    public class EmailSenderService_Tests
    {
        private Mock<IOptions<AuthMessageSenderOptions>> _mockOptions;
        private Mock<ILogger<EmailSender>> _mockLogger;
        private Mock<SendGridClient> _mockSendGridClient;
        private EmailSender _emailSender;

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<AuthMessageSenderOptions>>();
            _mockLogger = new Mock<ILogger<EmailSender>>();
            _mockSendGridClient = new Mock<SendGridClient>("fake-api-key");

            _mockOptions.Setup(o => o.Value).Returns(new AuthMessageSenderOptions
            {
                SendGridKey = "fake-api-key",
                SendGridFromEmail = "test@example.com"
            });

            _emailSender = new EmailSender(_mockOptions.Object, _mockLogger.Object);
        }

        [Test]
        public void SendEmailAsync_ShouldThrowExceptionWhenSendGridKeyIsMissing()
        {
            // Arrange
            _mockOptions.Setup(o => o.Value).Returns(new AuthMessageSenderOptions
            {
                SendGridKey = null // Missing API key
            });

            _emailSender = new EmailSender(_mockOptions.Object, _mockLogger.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _emailSender.SendEmailAsync("recipient@example.com", "Test Subject", "Test Message"));
            Assert.That(ex.Message, Is.EqualTo("Null SendGridKey"));
        }
    }
}