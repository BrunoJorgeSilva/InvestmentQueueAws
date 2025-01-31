//using Moq;
//using Microsoft.Extensions.Logging;
//using CountriesApi.Application.Interfaces;
//using CountriesApi.Application.Services;
//using CountriesApi.Domain.ResponseObjects.DTOs;

//public class InvesmentServiceTests
//{
//    private readonly Mock<IInvesmentIntegration> _mockInvesmentIntegration;
//    private readonly Mock<ILogger<InvesmentService>> _mockLogger;
//    private readonly InvesmentService _invesmentService;

//    public InvesmentServiceTests()
//    {
//        _mockInvesmentIntegration = new Mock<IInvesmentIntegration>();
//        _mockLogger = new Mock<ILogger<InvesmentService>>();

//        _invesmentService = new InvesmentService(
//            _mockInvesmentIntegration.Object,
//            _mockLogger.Object
//        );
//    }

//    [Fact]
//    public async Task Invest_ShouldReturnSuccess_WhenInvestmentIsSuccessful()
//    {
//        // Arrange
//        var investmentDto = new InvestmentDto { TransactionTrackerCode = new HashCode() };
//        _mockInvesmentIntegration
//            .Setup(x => x.Invest(investmentDto))
//            .ReturnsAsync(true);

//        // Act
//        var result = await _invesmentService.Invest(investmentDto);

//        // Assert
//        Assert.True(result.IsSuccess);
//        Assert.True(result.Value);
//        Assert.Null(result.ErrorMessage);

//        // Verify logging
//        _mockLogger.Verify(
//            x => x.Log(
//                LogLevel.Information,
//                It.IsAny<EventId>(),
//                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Starting to Invest {investmentDto.TransactionTrackerCode}")),
//                null,
//                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
//            Times.AtLeastOnce
//        );
//    }


//    [Fact]
//    public async Task Invest_ShouldReturnFailure_WhenInvestmentFails()
//    {
//        // Arrange
//        var investmentDto = new InvestmentDto { TransactionTrackerCode = new HashCode() };
//        _mockInvesmentIntegration
//            .Setup(x => x.Invest(investmentDto))
//            .ReturnsAsync(false);

//        // Act
//        var result = await _invesmentService.Invest(investmentDto);

//        // Assert
//        Assert.False(result.IsSuccess);
//        Assert.False(result.Value);
//        Assert.Equal("A problem ocurred trying to Investwith the investment, please try again later.", result.ErrorMessage);
//        _mockLogger.Verify(
//        x => x.Log(
//        LogLevel.Information,
//        It.IsAny<EventId>(),
//        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Result of the investment {investmentDto.TransactionTrackerCode}")),
//        null,
//        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
//        Times.AtLeastOnce);
//    }

//    [Fact]
//    public async Task Invest_ShouldReturnFailure_WhenExceptionOccurs()
//    {
//        // Arrange
//        var investmentDto = new InvestmentDto { TransactionTrackerCode = new HashCode() };
//        _mockInvesmentIntegration
//            .Setup(x => x.Invest(investmentDto))
//            .ThrowsAsync(new Exception("Integration error"));

//        // Act
//        var result = await _invesmentService.Invest(investmentDto);

//        // Assert
//        Assert.False(result.IsSuccess);
//        Assert.False(result.Value);
//        Assert.Equal("Error: Integration error", result.ErrorMessage);
//        _mockLogger.Verify(
//        x => x.Log(
//        LogLevel.Error,
//        It.IsAny<EventId>(),
//        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error: Integration error")),
//        It.IsAny<Exception>(),
//        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
//        Times.Once);
//    }
//}
