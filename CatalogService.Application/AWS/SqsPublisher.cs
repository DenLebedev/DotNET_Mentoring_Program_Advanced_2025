using System.Text.Json;
using Amazon.SQS;
using CatalogService.Application.Intefaces;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Application.AWS
{
    public class SqsPublisher : ISqsPublisher
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public SqsPublisher(IAmazonSQS sqs, IConfiguration config)
        {
            _sqs = sqs;
            _queueUrl = config["AWS:CatalogItemUpdatesQueueUrl"];
        }

        public async Task PublishCatalogItemUpdatedAsync(CatalogItemUpdatedMessage message)
        {
            var body = JsonSerializer.Serialize(message);
            await _sqs.SendMessageAsync(_queueUrl, body);
        }
    }
}
