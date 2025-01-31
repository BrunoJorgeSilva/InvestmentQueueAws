using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using CountriesApi.Application.Interfaces;
using CountriesApi.Domain.ResponseObjects.DTOs;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CountriesApi.Infrastructure.External
{
    public class InvestmentIntegration : IInvesmentIntegration
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string? _queueUrl;
        private readonly ILogger<InvestmentIntegration> _logger;

        public InvestmentIntegration(ILogger<InvestmentIntegration> logger, IConfiguration configuration)
        {
            _logger = logger;
            _queueUrl = configuration["AWS:QueueUrl"];
            var accessKey = configuration["AWS:AccessKey"];
            var secretKey = configuration["AWS:SecretKey"];
            var region = configuration["AWS:Region"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<bool> Invest(InvestmentDto investmentDto)
        {
            try
            {
                var jsonMessage = JsonConvert.SerializeObject(investmentDto);

                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MessageBody = jsonMessage,
                    MessageGroupId = "InvestmentGroup",  
                    MessageDeduplicationId = Guid.NewGuid().ToString()  
                };
                var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

                if (sendMessageResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation("Message sent successfully to SQS.");
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send message to SQS.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while sending the message: {ex.Message}");
                return false;
            }
        }
    }
}
