using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CartingService.Listeners
{
    public class CatalogItemUpdateListener : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private string _queueUrl;
        private readonly ILogger<CatalogItemUpdateListener> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public CatalogItemUpdateListener(
            IAmazonSQS sqs,
            IConfiguration configuration,
            ILogger<CatalogItemUpdateListener> logger,
            IServiceScopeFactory scopeFactory)
        {
            _sqs = sqs;
            _logger = logger;
            _scopeFactory = scopeFactory;
            _configuration = configuration;

            _logger.LogInformation("Using SQS Queue URL: {QueueUrl}", _queueUrl);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(_queueUrl))
            {
                var queueName = _configuration["AWS:CatalogItemUpdatesQueueName"];
                var response = await _sqs.GetQueueUrlAsync(queueName);
                _queueUrl = response.QueueUrl;
                _logger.LogInformation("Resolved Queue URL: {QueueUrl}", _queueUrl);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 10
                };

                var response = await _sqs.ReceiveMessageAsync(request, stoppingToken);

                var messages = response?.Messages;
                _logger.LogInformation("Received {Count} messages from SQS", messages?.Count ?? 0);

                // Safely skip if no messages
                if (messages == null || messages.Count == 0)
                {
                    _logger.LogInformation("No messages received from SQS.");
                    continue;
                }

                foreach (var message in messages)
                {
                    try
                    {
                        _logger.LogInformation("Raw SQS message: {Body}", message.Body);

                        var update = JsonSerializer.Deserialize<CatalogItemUpdatedMessage>(
                            message.Body,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        if (update == null)
                        {
                            _logger.LogWarning("Deserialized message is null. Skipping.");
                            continue;
                        }

                        if (string.IsNullOrEmpty(update.Name) || update.Price == null)
                        {
                            _logger.LogWarning("Deserialized message has null or missing fields. Skipping. Id={Id}, Name={Name}, Price={Price}",
                                update.Id, update.Name, update.Price);
                            continue;
                        }

                        using var scope = _scopeFactory.CreateScope();
                        var cartService = scope.ServiceProvider.GetRequiredService<ICartBL>();

                        _logger.LogInformation("Calling UpdateItemInAllCartsAsync with Id={Id}, Name={Name}, Price={Price}",
                            update.Id, update.Name, update.Price);

                        await cartService.UpdateItemInAllCartsAsync(update.Id, update.Name, update.Price);

                        await _sqs.DeleteMessageAsync(_queueUrl, message.ReceiptHandle, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process message");
                    }
                }
            }
        }
    }
}
