using System.Collections.Generic;
using System.Threading.Tasks;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Application.Services;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using Moq;
using Xunit;

namespace InvestmentConsumer.Tests
{
    public class QueueServiceTests
    {
        private readonly Mock<IQueueIntegration> _queueIntegrationMock;
        private readonly QueueService _queueService;

        public QueueServiceTests()
        {
            _queueIntegrationMock = new Mock<IQueueIntegration>();
            _queueService = new QueueService(_queueIntegrationMock.Object);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_ShouldReturnListOfInvestments_WhenMessagesAreAvailable()
        {
            // Arrange
            var investmentList = new List<InvestmentDto>
            {
                new InvestmentDto { ClientId = 1, InvesmentId = 100 },
                new InvestmentDto { ClientId = 2, InvesmentId = 200 }
            };
            _queueIntegrationMock.Setup(q => q.ReceiveMessagesAsync())
                                 .ReturnsAsync(investmentList);

            // Act
            var result = await _queueService.ReceiveMessagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_ShouldReturnEmptyList_WhenNoMessagesAvailable()
        {
            // Arrange
            _queueIntegrationMock.Setup(q => q.ReceiveMessagesAsync())
                                 .ReturnsAsync(new List<InvestmentDto>());

            // Act
            var result = await _queueService.ReceiveMessagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
