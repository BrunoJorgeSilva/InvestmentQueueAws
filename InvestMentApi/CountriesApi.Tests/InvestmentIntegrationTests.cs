using System;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using CountriesApi.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CountriesApi.Infrastructure.External.Tests
{
    public class InvestmentIntegrationTests
    {
        private readonly Mock<IAmazonSQS> _mockSqsClient;
        private readonly Mock<ILogger<InvestmentIntegration>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly InvestmentIntegration _investmentIntegration;

        public InvestmentIntegrationTests()
        {
            _mockSqsClient = new Mock<IAmazonSQS>();
            _mockLogger = new Mock<ILogger<InvestmentIntegration>>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Configuração simulada
            _mockConfiguration.Setup(config => config["AWS:QueueUrl"]).Returns("https://sqs.us-east-2.amazonaws.com/123456789012/TestQueue");
            _mockConfiguration.Setup(config => config["AWS:AccessKey"]).Returns("fake-access-key");
            _mockConfiguration.Setup(config => config["AWS:SecretKey"]).Returns("fake-secret-key");
            _mockConfiguration.Setup(config => config["AWS:Region"]).Returns("us-east-2");

            _investmentIntegration = new InvestmentIntegration(_mockLogger.Object, _mockConfiguration.Object);

            // Usando reflection para injetar o cliente SQS mockado
            typeof(InvestmentIntegration)
                .GetField("_sqsClient", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(_investmentIntegration, _mockSqsClient.Object);
        }

        [Fact]
        public async Task Invest_ShouldReturnTrue_WhenMessageIsSentSuccessfully()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 1, Amount = 1000 };
            var sendMessageResponse = new SendMessageResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };

            _mockSqsClient
                .Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(sendMessageResponse);

            // Act
            var result = await _investmentIntegration.Invest(investmentDto);

            // Assert
            Assert.True(result);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Message sent successfully to SQS.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invest_ShouldReturnFalse_WhenMessageFailsToSend()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 2, Amount = 2000 };
            var sendMessageResponse = new SendMessageResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            _mockSqsClient
                .Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ReturnsAsync(sendMessageResponse);

            // Act
            var result = await _investmentIntegration.Invest(investmentDto);

            // Assert
            Assert.False(result);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to send message to SQS.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invest_ShouldReturnFalse_WhenExceptionOccurs()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 3, Amount = 3000 };

            _mockSqsClient
                .Setup(sqs => sqs.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
                .ThrowsAsync(new Exception("SQS error"));

            // Act
            var result = await _investmentIntegration.Invest(investmentDto);

            // Assert
            Assert.False(result);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while sending the message: SQS error")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
