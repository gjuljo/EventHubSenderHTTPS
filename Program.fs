module SenderHTTPS

open FSharp.Data
open System
open System.Web
open System.Text
open System.Globalization

type EventHubNamespace = EventHubNamespace of String
type EventHubHostname  = EventHubHostname  of String
type EventHub          = EventHub          of String
type EventHubURI       = EventHubURI       of String
type Key               = Key               of String
type KeyName           = KeyName           of String
type SASToken          = SASToken          of String
type Message           = Message           of String

type EventHubConfig = {
    Namespace : string
    Hostname  : string
    Eventhub  : string
    Key       : string
    KeyName   : string
}

let config = {
    Namespace = Environment.GetEnvironmentVariable "EVENTHUB_NAMESPACE"
    Hostname  = Environment.GetEnvironmentVariable "EVENTHUB_HOST"
    Eventhub  = Environment.GetEnvironmentVariable "EVENTHUB_NAME"
    Key       = Environment.GetEnvironmentVariable "EVENTHUB_KEY"
    KeyName   = Environment.GetEnvironmentVariable "EVENTHUB_KEYNAME"
}

let createEventHubUri (EventHubNamespace namesp) (EventHub topic) =
    let uri = sprintf "%s/%s" namesp topic
    EventHubURI uri

let createToken (EventHubURI uri) (KeyName keyName) (Key key) =
    let sinceEpoch   = DateTime.Now - (new DateTime(1970,1,1))
    let week         = 60.0 * 60.0 * 24.0 * 7.0
    let expiry       = (sinceEpoch.TotalSeconds + week) |> Convert.ToInt32 |> Convert.ToString
    let stringToSign = HttpUtility.UrlEncode(uri) + "\n" + expiry
    let keyBytes     = Encoding.UTF8.GetBytes(key)
    use hmac         = new System.Security.Cryptography.HMACSHA256(keyBytes)
    let signature    = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)))
    let sasToken     = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", HttpUtility.UrlEncode(uri), HttpUtility.UrlEncode(signature), expiry, keyName)
    SASToken sasToken


let sendTextMessage (EventHubHostname hostname) (EventHub topic) (SASToken tk) (Message msg) =
    let url      = sprintf "https://%s/%s/messages" hostname topic
    let response = Http.Request(url, httpMethod = "POST",
                                        headers = [
                                            "Content-Type" , "application/atom+xml"
                                            "key"          , "entry"
                                            "charset"      , "utf-8"
                                            "Authorization", tk
                                        ],
                                        body = TextRequest msg)
    response


[<EntryPoint>]
let main argv =
    let eventHubName  = EventHubNamespace config.Namespace
    let eventHubTopic = EventHub          config.Eventhub
    let key           = Key               config.Key
    let keyName       = KeyName           config.KeyName
    let uri           = createEventHubUri eventHubName eventHubTopic
    let tk            = createToken uri keyName key
    let eventHubHost  = match config.Hostname with
                        | "" | null -> EventHubHostname config.Namespace
                        | _         -> EventHubHostname config.Hostname
   
    let message       = match argv with
                        | [|msg|] -> Message msg
                        | _       -> Message "this is a default message"

    let response = sendTextMessage eventHubHost eventHubTopic tk message
    (response.StatusCode,response.Body.ToString()) ||> printfn "%i %s" 

    0 // return an integer exit code
