using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Amazon.SQS;
using MySqsListener;

var builder = WebApplication.CreateBuilder(args);
var awsCredentials = new BasicAWSCredentials("test", "test");
builder.Services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(awsCredentials, new AmazonDynamoDBConfig
{
    ServiceURL = "http://localhost:4566",
    AuthenticationRegion = "us-west-2"
}));
builder.Services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient(awsCredentials, new AmazonSQSConfig
{
    ServiceURL = "http://localhost:4566",
    AuthenticationRegion = "us-west-2"
}));

builder.Services.AddHostedService<SqsListener>(sp =>
{
    var sqsClient = sp.GetRequiredService<IAmazonSQS>();
    var dynamoDbClient = sp.GetRequiredService<IAmazonDynamoDB>();
    return new SqsListener(sqsClient, "MyQueue", dynamoDbClient);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/messages", async (IAmazonDynamoDB dynamoDbClient) =>
    {
        var table = Table.LoadTable(dynamoDbClient, "MyTable");
        var scanFilter = new ScanFilter();
        var search = table.Scan(scanFilter);

        var documentList = new List<Document>();
        while (!search.IsDone)
        {
            documentList.AddRange(await search.GetNextSetAsync());
        }

        return documentList.Select(doc => doc.ToJson());
    })
    .WithName("GetSqsMessages")
    .WithOpenApi();

await app.RunAsync();