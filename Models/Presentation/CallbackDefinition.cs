using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;

/// <summary>
/// Callback - defines where and how we want our callback.
/// url - points back to us
/// state - something we pass that we get back in the callback event. We use it as a correlation id
/// headers - any additional HTTP headers you want to pass to the VC Client API. 
/// The values you pass will be returned, as HTTP Headers, in the callback
public class CallbackDefinition
{
    /// <summary>
    /// URI to the callback endpoint of your application. The URI must point to a reachable endpoint on the internet otherwise the service will throw callback URL unreadable error. 
    /// Accepted formats IPv4, IPv6 or DNS resolvable hostname
    /// </summary>  
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// Correlates the callback event with the state passed in the original payload.
    /// </summary>  
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// Optional. You can include a collection of HTTP headers required by the receiving end of the POST message. 
    // The current supported header values are the api-key or the Authorization headers. 
    /// Any other header will throw an invalid callback header error
    /// </summary>  
    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; }
}