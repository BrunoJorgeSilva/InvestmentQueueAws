using Amazon.Runtime;
using Amazon.SQS;
using Amazon;
using Amazon.SQS.Model;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Domain.ResponseObjects.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace InvestmentConsumer.Infrastructure.QueueIntegration
{
    public class QueueIntegration : IQueueIntegration
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string? _queueUrl;
        private readonly ILogger<QueueIntegration> _logger;

        public QueueIntegration(ILogger<QueueIntegration> logger, IConfiguration configuration)
        {
            _logger = logger;
            _queueUrl = configuration["AWS:QueueUrl"];
            var accessKey = configuration["AWS:AccessKey"];
            var secretKey = configuration["AWS:SecretKey"];
            var region = configuration["AWS:Region"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<List<InvestmentDto>> ReceiveMessagesAsync()
        {
            _logger.LogInformation($"[QueueIntegration.ReceiveMessagesAsync] Starting to get messages from queue at {DateTime.Now}", DateTime.Now);
            List<InvestmentDto> investments = new List<InvestmentDto>();
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 20
            };

            try
            {
                var receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
                if (receiveMessageResponse.Messages?.Count > 0)
                {
                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        _logger.LogInformation($"[QueueIntegration.ReceiveMessagesAsync] Starting to process the message: {message.Body}", message);

                        try
                        {
                            InvestmentDto investment = InvestmentDto.Desserialize(message.Body);
                            investments.Add(investment);
                        }
                        catch(Exception ex) 
                        {
                            _logger.LogError($"[QueueIntegration.ReceiveMessagesAsync] Error Desserializing object: {ex.Message}", ex);
                            continue;
                        }
                        finally 
                        {
                            await DeleteMessageFromQueueAsync(message.ReceiptHandle);
                        }
                    }
                    return investments;
                }
                _logger.LogInformation($"[QueueIntegration.ReceiveMessagesAsync] No Messages found, end of job at: {DateTime.Now}", DateTime.Now);
                return investments;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[QueueIntegration.ReceiveMessagesAsync] Error Desserializing object: {ex.Message}", ex);
                return investments;
            }
        }

        private async Task DeleteMessageFromQueueAsync(string receiptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = _queueUrl,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
            Console.WriteLine("Message deleted from queue.");
        }
    }
}

