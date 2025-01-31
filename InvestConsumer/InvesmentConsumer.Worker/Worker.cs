using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Domain.Entities;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;

namespace InvesmentConsumer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;
        

        public Worker(IServiceProvider serviceProvider, 
                      ILogger<Worker> logger) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _queueService = scope.ServiceProvider.GetRequiredService<IQueueIntegration>();
                    var _investmentService = scope.ServiceProvider.GetRequiredService<IInvestmentService>();

                    try
                    {
                        var investments = await _queueService.ReceiveMessagesAsync();
                        if (investments == null || investments.Count <= 0) { continue; }

                        foreach (var investment in investments) 
                        {
                            _logger.LogInformation($"[Worker.ExecuteTaskAsync] Starting to invest to the client {investment.ClientId}", investment);
                            var invested = await _investmentService.Invest(investment);
                            _logger.LogInformation($"[Worker.ExecuteTaskAsync] Result of the investment to the client {investment.ClientId}: {invested.Value}", investment);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[Worker.ExecuteTaskAsync] Error: {ex.Message}", ex);
                        continue;
                    }

                }

                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
        }
    }
}
