from aws_cdk import App

from app_stack import MyAppStack

app = App()
MyAppStack(app, "localstack-demo")

app.synth()
