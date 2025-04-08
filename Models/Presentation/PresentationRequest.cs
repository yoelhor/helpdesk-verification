using System.Text.Json;
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Presentation;


/// <summary>
/// Verified ID presentation request
/// </summary>
public class PresentationRequest
{
    /// <summary>
    /// Your decentralized identifier (DID) of your verifier Microsoft Entra tenant. 
    /// </summary>  
    [JsonPropertyName("authority")]
    public string Authority { get; set; }

    /// <summary>
    /// Optional. Determines whether a QR code is included in the response of this request. 
    /// Present the QR code and ask the user to scan it. Scanning the QR code launches the authenticator app with this presentation request. 
    /// Possible values are true (default) or false. When you set the value to false, use the return url property to render a deep link.
    /// </summary>  
    [JsonPropertyName("includeQRCode")]
    public bool IncludeQRCode { get; set; }

    /// <summary>
    /// Provides information about the verifier.
    /// </summary>  
    [JsonPropertyName("registration")]
    public RequestRegistration Registration { get; set; }

    /// <summary>
    /// Mandatory. Allows the developer to update the UI during the verifiable credential presentation process. 
    /// When the user completes the process, continue the process after the results are returned to the application.
    /// </summary>  
    [JsonPropertyName("callback")]
    public CallbackDefinition Callback { get; set; }

    /// <summary>
    /// Optional. Determines whether a receipt should be included in the response of this request. 
    /// Possible values are true or false (default). The receipt contains the original payload sent from the authenticator to the Verified ID service. 
    /// The receipt is useful for troubleshooting or if you have the need to ge the full details of the payload.
    /// </summary>  
    [JsonPropertyName("includeReceipt")]
    public bool IncludeReceipt { get; set; }

    /// <summary>
    /// A collection of RequestCredential objects.
    /// </summary>  
    [JsonPropertyName("requestedCredentials")]
    public List<RequestedCredential> RequestedCredentials { get; set; }

    /// <summary>
    /// Serialize this object into a string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Serialize this object into HTML string
    /// </summary>
    /// <returns></returns>
    public string ToHtml()
    {
        return this.ToString().Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
    }
}