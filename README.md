# Project Title

Demo Project: Using LocalStack with SQS and DynamoDB

## Description

This project demonstrates the capabilities of using LocalStack to emulate AWS services such as SQS and DynamoDB.  
The project includes both .NET code and AWS CDK (Cloud Development Kit) code to create and interact with these services locally.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
- [AWS CDK](https://docs.aws.amazon.com/cdk/v2/guide/getting_started.html)
- [LocalStack](https://localstack.cloud/)
- [awslocal](https://github.com/localstack/awscli-local)
- [cdklocal](https://github.com/localstack/aws-cdk-local)
- [Docker](https://www.docker.com/get-started)

## Setup

### .NET Project

1. Clone the repository:
    ```sh
    git clone https://github.com/omri-ratson/localstack-dotnet-demo.git
    cd localstack-dotnet-demo
    ```

2. Restore the .NET dependencies:
    ```sh
    dotnet restore
    ```

3. Build the .NET project:
    ```sh
    dotnet build
    ```

### AWS CDK Project

1. Navigate to the `cdk` directory:
    ```sh
    cd cdk
    ```

2. Create a virtual environment and activate it:
    ```sh
    python -m venv .venv
    source .venv/bin/activate  # On Windows use `.venv\Scripts\activate`
    ```

3. Install the required dependencies:
    ```sh
    pip install -r requirements.txt
    ```

4. Synthesize the CDK stack:
    ```sh
    cdk synth
    ```

## Running the Project

### Start LocalStack
This project uses a special flavor of local stack called [localstack-persist](https://github.com/GREsau/localstack-persist) sense the free tier of localstack do not save state between restarts of the container.

1. Start LocalStack using Docker:
    ```sh
    cd Localstack
    docker-compose up
    ```

### Deploy CDK Stack

1. Deploy the CDK stack to LocalStack:
    ```sh
    cdklocal bootstrap --profile local
    ```

2. Deploy the CDK stack to LocalStack:
    ```sh
    cdklocal deploy --profile local
    ```

### Run .NET Application

1. Run the .NET application:
    ```sh
    dotnet run
    ```

## Project Structure

- `cdk/`: Contains the AWS CDK code for creating SQS and DynamoDB resources.
- `src/`: Contains the .NET code for interacting with the AWS resources.
- `cdk.json`: Configuration file for the CDK application.

## Usage

This project demonstrates how to:

- Create and configure SQS queues and DynamoDB tables using AWS CDK.
- Interact with these resources using a .NET application.
- Use LocalStack to emulate AWS services locally for development and testing.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.