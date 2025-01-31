using InvestmentConsumer.Application.Common;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Domain.Entities;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Logging;

namespace InvestmentConsumer.Application.Services
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvesmentRepository _investmentRepository;
        private readonly ILogger<InvestmentService> _logger;

        public InvestmentService(IInvesmentRepository investmentIntegration,
                              ILogger<InvestmentService> logger)
        {
            _investmentRepository = investmentIntegration;
            _logger = logger;
        }

        public async Task<Result<bool>> Invest(InvestmentDto investmentDto)
        {
            _logger.LogInformation($"[InvesmentService.Invest] Starting to Invest to ClientID {investmentDto.ClientId}, the investment {investmentDto.InvesmentId}", investmentDto);
            try
            {
                Investment investment = new Investment(investmentDto);
                bool invested = await _investmentRepository.Invest(investment);
                _logger.LogInformation($"[InvesmentService.Invest] Result of the investment : {invested}", investmentDto);

                if (!invested) 
                {
                    return Result<bool>.Failure("A problem ocurred trying to Investwith the investment, please try again later.", false);
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[InvesmentService.Invest] Error: {ex.Message}", ex);
                return Result<bool>.Failure($"Error: {ex.Message}", false);
            }
        }
    }
}
