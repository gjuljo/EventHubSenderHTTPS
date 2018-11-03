# EventHubSenderHTTPS
Simple Azure EventHub sender client written in F# (.net core)

## Overview
This is a very simple HTTPS sender written in F# that sends one message to Azure Event Hub.
The code leverages two libraries:
- [F# Data](http://fsharp.github.io/FSharp.Data/), for its easy to use HTTP client
- [FsConfig](https://github.com/demystifyfp/FsConfig), to get configuration options (i.e. with environment variables)

Of course you also need an existing [Azure Event Hub](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-about) where you want your message to be sent to. You can follow any of the [quickstart](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create) tutorials to create one.

## Configuration
You need the following four configuration options to connect to an Azure Event Hub: 

- Event Hubs Namespace (i.e. `myeventhubnamespace.servicebus.windows.net`)
- Event Hub Name (i.e. `myeventhub`)
- Event Hub Primary Key of a given Shared Access Policy (i.e. `TZGSLIQm65ko2ulRWVjcTRHj6eIBuPWd+w0DwVsnPek=`)
- Shared Access Policy Name (i.e. `RootManageSharedAccessKey`)

The program expects the following four environment variables to be set:

~~~~
EVENTHUB_NAMESPACE=myeventhubnamespace.servicebus.windows.net
EVENTHUB_NAME=myeventhub
EVENTHUB_KEY=TZGSLIQm65ko2ulRWVjcTRHj6eIBuPWd+w0DwVsnPek=
EVENTHUB_KEYNAME=RootManageSharedAccessKey
~~~~

You can set update either `run.bat` or `runsdk.bat` with the above values to get ready.

Anything you add as command line argument is used as message content. Without any input, the default message `this is a default message` is sent.
