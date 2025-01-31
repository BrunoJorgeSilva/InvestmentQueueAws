using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;

namespace InvestmentConsumer.Application.Services
{
    public class QueueService : IQueueService
    {
        private readonly IQueueIntegration _queueIntegration;

        public QueueService(IQueueIntegration queueIntegration)
        {
            _queueIntegration = queueIntegration;

        }
        public async Task<List<InvestmentDto>> ReceiveMessagesAsync()
        {
            return await _queueIntegration.ReceiveMessagesAsync();
        }
    }
}
