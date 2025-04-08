using System.Text.Json;
using System.Text.Json.Serialization;

namespace helpdesk_prove_request.Model.Callback;

/// <summary>
/// Base calls for Verified ID Client API callback for issuance and presentation 
/// </summary>
public class CallbackData : CallbackDataBase
{
    /// <summary>
    /// The verifiable credential user DID.
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; }

    /// <summary>
    /// Returns an array of Verified IDs requested. For each Verified ID, it provides:
    /// The Verified ID type(s).
    /// The issuer's DID
    /// The claims retrieved.
    /// The Verified ID issuer's domain.
    /// The Verified ID issuer's domain validation status.
    /// <summary>
    [JsonPropertyName("verifiedCredentialsData")]
    public List<CallbackVerifiedCredentialsData> VerifiedCredentialsData { get; set; }

    /// <summary>
    /// Optional. The receipt contains the original payload sent from the wallet to the Verifiable Credentials service. The receipt should be used for troubleshooting/debugging only. 
    /// The format in the receipt isn't fix and can change based on the wallet and version used.
    /// </summary>
    [JsonPropertyName("receipt")]
    public CallbackReceipt Receipt { get; set; }


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
    public new string ToHtml()
    {
        return this.ToString().Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
    }

    /// <summary>
    /// Deserialize a JSON string into CallbackData object
    /// </summary>
    /// <param name="JsonString">The JSON string to be loaded</param>
    /// <returns></returns>
    public static new CallbackData Parse(string JsonString)
    {
        return JsonSerializer.Deserialize<CallbackData>(JsonString)!;
    }

}
