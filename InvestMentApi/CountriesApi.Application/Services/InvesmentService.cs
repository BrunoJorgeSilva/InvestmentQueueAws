using CountriesApi.Application.Common;
using CountriesApi.Application.Interfaces;
using CountriesApi.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Logging;

namespace CountriesApi.Application.Services
{
    public class InvesmentService : IInvestmentService
    {
        private readonly IInvesmentIntegration _invesmentIntegration;
        private readonly ILogger<InvesmentService> _logger;

        public InvesmentService(IInvesmentIntegration invesmentIntegration,
                              ILogger<InvesmentService> logger)
        {
            _invesmentIntegration = invesmentIntegration;
            _logger = logger;
        }

        public async Task<Result<bool>> Invest(InvestmentDto investmentDto)
        {
            _logger.LogInformation($"[InvesmentService.Invest] Starting to Invest {investmentDto.ClientId}, {investmentDto.InvesmentId}", investmentDto);
            try
            {
                bool invested = await _invesmentIntegration.Invest(investmentDto);
                _logger.LogInformation($"[InvesmentService.Invest] Result of the investment {investmentDto.ClientId} : invested", investmentDto);

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
