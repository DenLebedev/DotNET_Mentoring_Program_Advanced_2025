using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using CatalogService.Application.Events;

namespace CatalogService.Infrastructure.Messaging;
public class SqsPublisher : IEventPublisher
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;

    public SqsPublisher(IAmazonSQS sqsClient, IConfiguration configuration)
    {
        _sqsClient = sqsClient;
        _queueUrl = configuration["AWS:CatalogItemUpdatesQueueUrl"];
    }

    public async Task PublishAsync<T>(T message)
    {
        var body = JsonSerializer.Serialize(message);
        await _sqsClient.SendMessageAsync(_queueUrl, body);
    }
}
