using System.Text.Json;
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;

/// <summary>
/// Base calls for Verified ID Client API callback for issuance and presentation 
/// </summary>
public class CallbackDataBase
{
    /// <summary>
    /// Mapped to the original request when the payload was posted to the Verified ID service.  
    /// </summary>  
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }

    /// <summary>
    /// The status returned for the request..  
    /// </summary>  
    [JsonPropertyName("requestStatus")]
    public string RequestStatus { get; set; }

    /// <summary>
    /// Returns the state value that you passed in the original payload.
    /// <summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// Contains information about the error
    /// </summary>
    [JsonPropertyName("error")]
    public CallbackError Error { get; set; }


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

    /// <summary>
    /// Deserialize a JSON string into CallbackBase object
    /// </summary>
    /// <param name="JsonString">The JSON string to be loaded</param>
    /// <returns></returns>
    public static CallbackDataBase Parse(string JsonString)
    {
        return JsonSerializer.Deserialize<CallbackDataBase>(JsonString);
    }

}
