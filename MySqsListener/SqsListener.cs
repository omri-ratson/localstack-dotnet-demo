using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace MySqsListener;

public class SqsListener(IAmazonSQS sqsClient, string queueName, IAmazonDynamoDB dynamoDbClient) : BackgroundService
{
    private readonly List<string> _messageAttributeNames = ["All"];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = await sqsClient.GetQueueUrlAsync(queueName, stoppingToken);
        var receiveRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageAttributeNames = _messageAttributeNames,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 20
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var messageResponse = await sqsClient.ReceiveMessageAsync(receiveRequest, stoppingToken);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                //Do some logging or handling?
                continue;
            }

            foreach (var message in messageResponse.Messages)
            {
                var table = Table.LoadTable(dynamoDbClient, "MyTable");
                var document = new Document
                {
                    ["MessageId"] = message.MessageId,
                    ["Body"] = message.Body,
                    ["Timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };
                await table.PutItemAsync(document, stoppingToken);

                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = queueUrl.QueueUrl,
                    ReceiptHandle = message.ReceiptHandle
                };
                await sqsClient.DeleteMessageAsync(deleteMessageRequest, stoppingToken);
            }
        }
    }
}