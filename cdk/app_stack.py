from aws_cdk import (
    Stack,
    aws_sqs as sqs,
    aws_dynamodb as dynamodb,
    Duration, RemovalPolicy, App,
)
from constructs import Construct


class MyAppStack(Stack):

    def __init__(self, scope: Construct, id: str, **kwargs) -> None:
        super().__init__(scope, id, **kwargs)

        # Create an SQS queue
        queue = sqs.Queue(self, "MyQueueId",
                          queue_name="MyQueue",
                          visibility_timeout=Duration.seconds(10),
                          )

        # Create a DynamoDB table
        table = dynamodb.Table(self, "MyTableId",
                               table_name="MyTable",
                               partition_key=dynamodb.Attribute(
                                   name="MessageId",
                                   type=dynamodb.AttributeType.STRING
                               ),
                               sort_key=dynamodb.Attribute(
                                   name="Timestamp",
                                   type=dynamodb.AttributeType.STRING
                               ),
                               removal_policy=RemovalPolicy.DESTROY,
                               )


app = App()
MyAppStack(app, "MyAppStack")
app.synth()
