using System;
using System.Threading.Tasks;
using InvestmentConsumer.Application.Common;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Application.Services;
using InvestmentConsumer.Domain.Entities;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InvestmentConsumer.Tests
{
    public class InvestmentServiceTests
    {
        private readonly Mock<IInvesmentRepository> _investmentRepositoryMock;
        private readonly Mock<ILogger<InvestmentService>> _loggerMock;
        private readonly InvestmentService _investmentService;

        public InvestmentServiceTests()
        {
            _investmentRepositoryMock = new Mock<IInvesmentRepository>();
            _loggerMock = new Mock<ILogger<InvestmentService>>();
            _investmentService = new InvestmentService(_investmentRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Invest_ShouldReturnSuccess_WhenInvestmentSucceeds()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 123, InvesmentId = 456 };
            _investmentRepositoryMock.Setup(repo => repo.Invest(It.IsAny<Investment>()))
                                     .ReturnsAsync(true);

            // Act
            var result = await _investmentService.Invest(investmentDto);

            // Assert
            Assert.True(result.Value);
            Assert.True(result.IsSuccess);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task Invest_ShouldReturnFailure_WhenInvestmentFails()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 123, InvesmentId = 456 };
            _investmentRepositoryMock.Setup(repo => repo.Invest(It.IsAny<Investment>()))
                                     .ReturnsAsync(false);

            // Act
            var result = await _investmentService.Invest(investmentDto);

            // Assert
            Assert.False(result.Value);
            Assert.False(result.IsSuccess);
            Assert.Equal("A problem ocurred trying to Investwith the investment, please try again later.", result.ErrorMessage);
        }

        [Fact]
        public async Task Invest_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var investmentDto = new InvestmentDto { ClientId = 123, InvesmentId = 456 };
            _investmentRepositoryMock.Setup(repo => repo.Invest(It.IsAny<Investment>()))
                                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _investmentService.Invest(investmentDto);

            // Assert
            Assert.False(result.Value);
            Assert.False(result.IsSuccess);
            Assert.Contains("Database error", result.ErrorMessage);
        }
    }
}
