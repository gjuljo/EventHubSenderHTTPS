@setlocal
@set EVENTHUB_NAMESPACE=myeventhubnamespace.servicebus.windows.net
@set EVENTHUB_NAME=myeventhub
@set EVENTHUB_KEY=TZGSLIQm65ko2ulRWVjcTRHj6eIBuPWd+w0DwVsnPek=
@set EVENTHUB_KEYNAME=RootManageSharedAccessKey
dotnet run "%*"
@endlocal